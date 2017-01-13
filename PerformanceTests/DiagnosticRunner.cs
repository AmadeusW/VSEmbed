using System;
using System.Linq;
using System.Threading;
using System.Windows;
using VSEmbed;
using BenchmarkDotNet.Attributes;
using System.Reflection;

namespace PerformanceTests
{
	internal class DiagnosticRunner
	{
		internal static void Run<T>(string testName) where T: IDebuggableTest
		{
			var thread = new Thread(() =>
			{
				var testClass = (T)Activator.CreateInstance(typeof(T));
				var testMethods = typeof(T).GetMethods().Where(methodInfo => methodInfo.GetCustomAttributes(typeof(BenchmarkAttribute), true).Length > 0);
				var testMethod = testMethods.Single(n => n.Name == testName);
				new WpfApplication(testClass, testMethod).Run();
			});

			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		private class WpfApplication : Application
		{
			private readonly VSEmbed.DemoApp.MainWindow _mainWindow;
			private readonly IDebuggableTest _testClass;
			private readonly MethodInfo _testMethod;

			public WpfApplication(IDebuggableTest testClass, MethodInfo testMethod)
			{
				_testClass = testClass;
				_testMethod = testMethod;
				_mainWindow = new VSEmbed.DemoApp.MainWindow();
			}

			protected override void OnStartup(StartupEventArgs e)
			{
				_mainWindow.Show();
				_testClass.Setup();
				// TODO: allow SendKey with any window. Currently this won't work
				//_testMethod.Invoke(_testClass, new object[0]);
			}
		}
	}
}