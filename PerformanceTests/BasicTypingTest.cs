using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using VSEmbed;
using VSEmbed.DemoApp;

namespace PerformanceTests
{
	public class BasicTypingTest
	{
		private MainWindow _window;
		static BasicTypingTest()
		{
			VsLoader.Load(new Version(14, 0, 0, 0));
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
		}

		private static void initializeRoslynForegroundThreadDataObject()
		{
			var currentThread = Thread.CurrentThread;
			var assembly = Assembly.Load("Microsoft.CodeAnalysis.EditorFeatures");
			var t_foregroundThreadData = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadData");
			var m_createDefault = t_foregroundThreadData.GetMethod("CreateDefault", BindingFlags.Static | BindingFlags.NonPublic);
			int foregroundThreadDataKind = 4;
			var result = m_createDefault.Invoke(null, new object[] { foregroundThreadDataKind });
			var t_foregroundThreadAffinitizedObject = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadAffinitizedObject");
			var props = t_foregroundThreadAffinitizedObject.GetProperties().ToList();
			var methods = t_foregroundThreadAffinitizedObject.GetMethods();
			var m_currentForegroundThreadData = t_foregroundThreadAffinitizedObject.GetProperty("CurrentForegroundThreadData", BindingFlags.Static | BindingFlags.NonPublic);
			m_currentForegroundThreadData.SetValue(null, result);
		}

		public enum ContentType { text, CSharp}
		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		[Setup]
		public void Setup()
		{
			//Can we please not have to do this?
			initializeRoslynForegroundThreadDataObject();
			_window = new MainWindow();
			_window.Show();
			if(CurrentContentType == ContentType.text)
			{
				_window.SetContentType(nameof(ContentType.text));
			}
			else if(CurrentContentType == ContentType.CSharp)
			{
				_window.SetContentType(nameof(ContentType.CSharp));
			}

		}

		[Cleanup]
		public void Cleanup()
		{
			_window.Close();
			_window = null;
		}

		[Benchmark, STAThread]
		public void BasicTypingPerf()
		{
			_window.SendKeystrokes("namespace");
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
			_window.SendKey(System.Windows.Input.Key.Back);
		}
	}
}
