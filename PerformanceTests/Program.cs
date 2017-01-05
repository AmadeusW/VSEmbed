using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VSEmbed;

namespace PerformanceTests
{
	class Program
	{
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

		[STAThread]
		static void Main(string[] args)
		{
			//var x = new BasicTypingTest();
			//x.Setup();
			//x.BasicTypingPerf();

			var summary = BenchmarkRunner.Run<BasicTypingTest>();

			//var thread = new Thread(() =>
			//{
			//	VsLoader.Load(new Version(14, 0, 0, 0));
			//	VsServiceProvider.Initialize();
			//	VsMefContainerBuilder.CreateDefault().Build();

			//	//Can we please not have to do this?
			//	initializeRoslynForegroundThreadDataObject();
			//	var window = new VSEmbed.DemoApp.MainWindow();
			//	new WpfApplication(window).Run();
			//});

			//thread.SetApartmentState(ApartmentState.STA);
			//thread.Start();
			//thread.Join();
		}
		private class WpfApplication : Application
		{
			private readonly VSEmbed.DemoApp.MainWindow _mainWindow;

			public WpfApplication(VSEmbed.DemoApp.MainWindow mainWindow)
			{
				_mainWindow = mainWindow;
			}

			protected override void OnStartup(StartupEventArgs e)
			{
				_mainWindow.Show();
				//_mainWindow.SendKeyInput("{");
			}
		}
	}
}
