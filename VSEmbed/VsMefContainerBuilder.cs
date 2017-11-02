using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Composition;

using MEFv1 = System.ComponentModel.Composition;
using MEFv3 = Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.Imaging;

namespace VSEmbed
{
	///<summary>Creates the MEF composition container used by the editor services.  This type is immutable</summary>
	/// <remarks>Stolen, with much love and gratitude, from @JaredPar's EditorUtils.</remarks>
	public class VsMefContainerBuilder
	{
		readonly MEFv3.ComposableCatalog catalog;

		static readonly MEFv3.PartDiscovery partDiscovery = MEFv3.PartDiscovery.Combine(
			new MEFv3.AttributedPartDiscovery(Resolver.DefaultInstance, isNonPublicSupported: true),
			new MEFv3.AttributedPartDiscoveryV1(Resolver.DefaultInstance));

		private static readonly string[] excludedTypes = {
			// This uses IVsUIShell, which I haven't implemented, to show dialog boxes.
			// It also causes strange and fatal AccessViolations.
			"Microsoft.VisualStudio.Editor.Implementation.ExtensionErrorHandler",

			// This uses IOleUndoManager, which I don't have.  I replace it with @JaredPar's implementation.
			"Microsoft.VisualStudio.Editor.Implementation.Undo.VsUndoHistoryRegistry",

			// This uses IOleComponentManager, which I don't know how to implement.
			"Microsoft.VisualStudio.Editor.Implementation.Intellisense.VsWpfKeyboardTrackingService",

			// This uses IWpfKeyboardTrackingService, and I don't want Code Lens anyway (yet?)
			"Microsoft.VisualStudio.Language.Intellisense.Implementation.CodeLensAdornmentCache",
			"Microsoft.VisualStudio.Language.Intellisense.Implementation.CodeLensInterLineAdornmentTaggerProvider",

			// This uses COM services to try to read user settings, and I can't make that work.  I replace it with my own simple implementation.
			"Microsoft.VisualStudio.Editor.Implementation.DataStorageService",

			// I export my own direct SVsServiceProvider
			"Microsoft.VisualStudio.ComponentModelHost.VsComponentModelHostExporter"
		};

		private VsMefContainerBuilder(MEFv3.ComposableCatalog catalog)
		{
			this.catalog = catalog;
		}

		public static VsMefContainerBuilder CreateDefault()
		{
			var assemblyNames = new string[] {
				"Microsoft.CodeAnalysis.CSharp.EditorFeatures",
				"Microsoft.CodeAnalysis.CSharp.Features",
				"Microsoft.CodeAnalysis.CSharp.Workspaces",
				"Microsoft.CodeAnalysis.EditorFeatures",
				"Microsoft.CodeAnalysis.EditorFeatures.Text",
				"Microsoft.CodeAnalysis.Features",
				"Microsoft.CodeAnalysis.VisualBasic.EditorFeatures",
				"Microsoft.CodeAnalysis.VisualBasic.Features",
				"Microsoft.CodeAnalysis.VisualBasic.Workspaces",
				"Microsoft.CodeAnalysis.Workspaces.Desktop",
				"Microsoft.CodeAnalysis.Workspaces",

				"Microsoft.VisualStudio.Language.CallHierarchy",
				"Microsoft.VisualStudio.Language.NavigateTo.Interfaces",

				"Microsoft.VisualStudio.Platform.VSEditor",
				"Microsoft.VisualStudio.Text.Logic",
				"Microsoft.VisualStudio.Text.UI",
				"Microsoft.VisualStudio.Text.UI.Wpf",
				"Microsoft.VisualStudio.Editor.Implementation",
				"Microsoft.VisualStudio.Shell.TreeNavigation.HierarchyProvider",

				"Microsoft.VisualStudio.Composition.Configuration",
				"VSEmbed.Roslyn",
				"BasicUndo",
			};

			//FileNames and AssemblyNames -> Assemblies
			var assemblies = new List<Assembly>();
			assemblies.Add(typeof(VsMefContainerBuilder).Assembly);
			assemblies.AddRange(assemblyNames.Select(n => Assembly.Load(n)));

			//Assemblies -> Types
			var types = assemblies.SelectMany(a => a.GetTypes().Where(t => !excludedTypes.Contains(t.FullName))).ToList();
			// IWaitIndicator is internal, so I have no choice but to use the existing
			// implementation. The rest of Microsoft.VisualStudio.LanguageServices.dll
			// exports lots of VS interop types that I don't want.
			types.Add(Type.GetType("Microsoft.VisualStudio.LanguageServices.Implementation.Utilities.VisualStudioWaitIndicator, " + "Microsoft.VisualStudio.LanguageServices"));
			// Enables rename (but breaks F12).
			types.Add(Type.GetType("Microsoft.VisualStudio.LanguageServices.Implementation.VisualStudioDocumentNavigationServiceFactory, " + "Microsoft.VisualStudio.LanguageServices"));
			// VS2015 Preview version of above type.
			types.Add(Type.GetType("Microsoft.VisualStudio.LanguageServices.Implementation.PreviewPane.PreviewPaneService, " + "Microsoft.VisualStudio.LanguageServices"));
			// Necessary (together with ugly Reflection) to use WorkCoordinator.HighPriorityProcessor.
			types.Add(Type.GetType("Microsoft.VisualStudio.LanguageServices.Implementation.VisualStudioDocumentTrackingServiceFactory, " + "Microsoft.VisualStudio.LanguageServices"));

			var containerBuilder = new VsMefContainerBuilder(MEFv3.ComposableCatalog.Create(Resolver.DefaultInstance))
				.WithCatalog(types);

			InitializeImageService();

			return containerBuilder;
		}

		private static void InitializeImageService()
		{
			const string manifest = "Microsoft.VisualStudio.ImageCatalog.imagemanifest";
			var library = ImageLibrary.Load(manifest, isDefault: true);
		}

		public VsMefContainerBuilder WithCatalog(IEnumerable<Type> types)
		{
			// Consumers are expected to build their MEF catalogs before setting
			// up the UI thread, so this should not create async deadlocks under
			// normal usage.
			return new VsMefContainerBuilder(catalog.AddParts(
				partDiscovery.CreatePartsAsync(types)
					.GetAwaiter()
					.GetResult()
					.ThrowOnErrors()
			));
		}

		///<summary>
		/// Creates a MEF container from this builder instance, and installs it into the global ServiceProvider.
		/// Editor factories will not work before this method is called.
		///</summary>
		public void Build()
		{
			var exportProvider = MEFv3.RuntimeComposition
				.CreateRuntimeComposition(MEFv3.CompositionConfiguration.Create(catalog).ThrowOnErrors())
				.CreateExportProviderFactory()
				.CreateExportProvider();

			var container = new ComponentModel(exportProvider);
			VsServiceProvider.Instance.SetMefContainer(container);
		}

		class ComponentModel : IComponentModel
		{
			public readonly MEFv3.ExportProvider ExportProvider;

			public ComponentModel(MEFv3.ExportProvider exportProvider)
			{
				this.ExportProvider = exportProvider;
			}

			public MEFv1.ICompositionService DefaultCompositionService
			{
				get { return ExportProvider.GetExport<MEFv1.ICompositionService>().Value; }
			}

			public MEFv1.Hosting.ExportProvider DefaultExportProvider
			{
				get { return ExportProvider.AsExportProvider(); }
			}

			public MEFv1.Primitives.ComposablePartCatalog DefaultCatalog { get { throw new NotSupportedException(); } }
			public MEFv1.Primitives.ComposablePartCatalog GetCatalog(string catalogName) { throw new NotSupportedException(); }

			public IEnumerable<T> GetExtensions<T>() where T : class { return ExportProvider.GetExportedValues<T>(); }
			public T GetService<T>() where T : class { return ExportProvider.GetExportedValue<T>(); }
		}
	}
}
