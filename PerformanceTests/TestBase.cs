using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSEmbed;
using VSEmbed.Contracts;

namespace PerformanceTests
{
	public enum ContentType { text, CSharp }

	public abstract class TestBase
	{
		protected IEmbeddedTextViewHost Host { get; private set; }
		
		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		static TestBase()
		{
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
			InitializeRoslynForegroundThreadDataObject();
		}

		internal static void InitializeRoslynForegroundThreadDataObject()
		{
			var assembly = Assembly.Load("Microsoft.CodeAnalysis.EditorFeatures");
			var t_foregroundThreadData = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadData");
			var m_createDefault = t_foregroundThreadData.GetMethod("CreateDefault", BindingFlags.Static | BindingFlags.NonPublic);
			int foregroundThreadDataKind = 4;
			var result = m_createDefault.Invoke(null, new object[] { foregroundThreadDataKind });
			var t_foregroundThreadAffinitizedObject = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadAffinitizedObject");
			var m_currentForegroundThreadData = t_foregroundThreadAffinitizedObject.GetProperty("CurrentForegroundThreadData", BindingFlags.Static | BindingFlags.NonPublic);
			m_currentForegroundThreadData.SetValue(null, result);
		}	

		[Setup]
		public void Setup()
		{
			Host = new VSEmbed.DemoApp.MainWindow();
			Host.Show();
			Host.SetContentType(CurrentContentType.ToString());
		}

		[Cleanup]
		public void Cleanup()
		{
			Host.Close();
			Host = null;
		}

		/// <summary>
		/// This method is used only in UI debugging of the test.
		/// Benchmark needs to use the Setup method.
		/// </summary>
		/// <param name="host"></param>
		internal void AttachToHost(IEmbeddedTextViewHost host)
		{
			Host = host;
			Host.SetContentType(CurrentContentType.ToString());
		}
	}
}
