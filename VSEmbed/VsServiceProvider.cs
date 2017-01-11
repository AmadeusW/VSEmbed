using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSEmbed.Services;
using OLE = Microsoft.VisualStudio.OLE.Interop;
using Shell = Microsoft.VisualStudio.Shell;
using System.Linq;

namespace VSEmbed
{
	///<summary>An out-of-process implementation of Visual Studio's singleton OLE ServiceProvider.</summary>
	///<remarks>
	/// Visual Studio services use this class, both through MEF SVsServiceProvider and
	/// Shell.ServiceProvider.GlobalProvider, to load COM services.  I need to provide
	/// every service used in editor and theme codepaths.  Most of the service methods
	/// are never actually called.
	/// This must be initialized before theme dictionaries or editor services are used
	///</remarks>
	public class VsServiceProvider : OLE.IServiceProvider, SVsServiceProvider {
		// Based on Microsoft.VisualStudio.ComponentModelHost.ComponentModel.DefaultCompositionContainer.
		// By implementing SVsServiceProvider myself, I skip an unnecessary call to GetIUnknownForObject.
		///<summary>Gets the singleton service provider instance.  This is exported to MEF.</summary>
		[Export(typeof(SVsServiceProvider))]
		public static VsServiceProvider Instance { get; private set; }

		///<summary>Creates the global service provider and populates it with the services we need.</summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "These objects become global and must not be disposed yet")]
		public static void Initialize() {
			// If we're in a real VS process, or if we already initialized, do nothing.
			if (Shell.ServiceProvider.GlobalProvider.GetService(typeof(SVsSettingsManager)) != null)
				return;

			var esm = ExternalSettingsManager.CreateForApplication(InstallationPath);
			var sp = new VsServiceProvider
			{
				serviceInstances = {
					// Used by Shell.ServiceProvider initialization
					{ typeof(SVsActivityLog).GUID, new StubVsActivityLog() },

					// Used by ColorThemeService
					{ typeof(SVsSettingsManager).GUID, new SettingsManagerWrapper(esm) },

					// Used by Shell.VsResourceKeys
					{ new Guid("45652379-D0E3-4EA0-8B60-F2579AA29C93"), new SimpleVsWindowManager() },

					// Used by KnownUIContexts
					{ typeof(IVsMonitorSelection).GUID, new StubVsMonitorSelection() },

					// Used by ShimCodeLensPresenterStyle
					{ typeof(SUIHostLocale).GUID, new SystemUIHostLocale() },
					{ typeof(SVsFontAndColorCacheManager).GUID, new StubVsFontAndColorCacheManager() },

					// Used by Roslyn's VisualStudioWaitIndicator
					{ typeof(SVsThreadedWaitDialogFactory).GUID, new BaseWaitDialogFactory() },

					// Used by Dev14's VsImageLoader, which is needed for Roslyn IntelliSense
					{ typeof(SVsAppId).GUID, new SimpleVsAppId() },

					// Used by KeyBindingHelper.GetKeyBinding, which is used by VSLightBulbPresenterStyle.
					{ typeof(SDTE).GUID, new StubDTE() },

					// Used by VsTaskSchedulerService; see below
					{ typeof(SVsShell).GUID, new StubVsShell() },

				}
			};

			Shell.ServiceProvider.CreateFromSetSite(sp);
			Instance = sp;

			// Add services that use IServiceProvider here
			sp.AddTaskSchedulerService();
		}

		private void AddTaskSchedulerService() {
			// Force its singleton instance property, used by VsTaskSchedulerService, to be set
			var package = new Microsoft.VisualStudio.Services.TaskSchedulerPackage();
			((IVsPackage)package).SetSite(this);

			// This ctor calls other services from the ServiceProvider, so it must be added after initialization.
			// VsIdleTimeScheduler uses VsShell.ShellPropertyChanged to wait for an OleComponentManager to exist,
			// then calls FRegisterComponent().  I don't know how to implement that, so my VsShell will not raise
			// event, leaving it in limbo.  This means that idle processing won't work; I don't know where that's
			// used.
			// Used by JoinableTaskFactory
			AddService(typeof(SVsTaskSchedulerService), Activator.CreateInstance(typeof(VsMenu).Assembly.GetType("Microsoft.VisualStudio.Services.VsTaskSchedulerService")));
		}

		///<summary>Gets the MEF IComponentModel installed in this ServiceProvider, if any.</summary>
		public IComponentModel ComponentModel { get; private set; }

		///<summary>Registers a MEF container in this ServiceProvider.</summary>
		///<remarks>
		/// This is used to provide the IComponentModel service, which is used by many parts of Roslyn and the editor.
		/// It's also used by our TextViewHost wrapper control to access the MEF container.
		///</remarks>
		public void SetMefContainer(IComponentModel container) {
			ComponentModel = container;
			AddService(typeof(SComponentModel), ComponentModel);
		}

		readonly Dictionary<Guid, object> serviceInstances = new Dictionary<Guid, object>();

		///<summary>Adds a new service to the provider (or replaces an existing service).</summary>
		public void AddService(Type serviceType, object instance) { AddService(serviceType.GUID, instance); }
		///<summary>Adds a new service to the provider (or replaces an existing service).</summary>
		public void AddService(Guid serviceGuid, object instance) {
			serviceInstances[serviceGuid] = instance;
		}

		int OLE.IServiceProvider.QueryService([ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFGUID")]ref Guid guidService, [ComAliasName("Microsoft.VisualStudio.OLE.Interop.REFIID")]ref Guid riid, out IntPtr ppvObject) {
			object result;
			if (!serviceInstances.TryGetValue(guidService, out result)) {
				ppvObject = IntPtr.Zero;
				return VSConstants.E_NOINTERFACE;
			}
			if (riid == VSConstants.IID_IUnknown) {
				ppvObject = Marshal.GetIUnknownForObject(result);
				return VSConstants.S_OK;
			}

			IntPtr unk = IntPtr.Zero;
			try {
				unk = Marshal.GetIUnknownForObject(result);
				result = Marshal.QueryInterface(unk, ref riid, out ppvObject);
			} finally {
				if (unk != IntPtr.Zero)
					Marshal.Release(unk);
			}
			return VSConstants.S_OK;
		}

		///<summary>Gets the specified service from the provider, or null if it has not been registered.</summary>
		public object GetService(Type serviceType) {
			object result;
			serviceInstances.TryGetValue(serviceType.GUID, out result);
			return result;
		}


		const string basePath = @"C:\Program Files (x86)\Microsoft Visual Studio";
		const string vsFileName = "devenv.exe";
		/// <summary>Gets a path to a Visual Studio 2015 installation.</summary>
		/// TODO: currently hard coded path to dev14. dev15 doesn't have certain settings that dev14 dlls expect
		private static string InstallationPath => @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe"; /*
		{
			get
			{
				var directories = Directory.GetDirectories(basePath).OrderBy(n => n.Length);
				var path = directories.Select(d => Directory.GetFiles(d, vsFileName, SearchOption.AllDirectories).FirstOrDefault()).First();
				return path;
			}
		}*/
	}
}
