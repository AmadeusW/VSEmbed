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

	public abstract class TestBase : IDebuggableTest
	{
		protected IEmbeddedTextViewHost Host { get; private set; }
		
		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		static TestBase()
		{
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
			initializeRoslynForegroundThreadDataObject();
		}

		private static void initializeRoslynForegroundThreadDataObject()
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

		void IDebuggableTest.AttachToHost(IEmbeddedTextViewHost host)
		{
			Host = host;
		}
	}
}
