using System;
using System.Linq;
using System.Threading;
using System.Windows;
using VSEmbed;
using BenchmarkDotNet.Attributes;
using System.Reflection;
using VSEmbed.Contracts;

namespace PerformanceTests
{
	/// <summary>
	/// Runs benchmark code in the UI context
	/// </summary>
	internal class DiagnosticRunner
	{
		internal static void Run<T>(string testName) where T: IDebuggableTest
		{
			var thread = new Thread(() =>
			{
				var testClass = (T)Activator.CreateInstance(typeof(T));
				testClass.Setup();
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
			private readonly IEmbeddedTextViewHost _mainWindow;
			private readonly IDebuggableTest _testClass;
			private readonly MethodInfo _testMethod;

			public WpfApplication(IDebuggableTest testClass, MethodInfo testMethod)
			{
				_testClass = testClass;
				_testMethod = testMethod;
				_mainWindow = new VSEmbed.DemoApp.MainWindow();
				_testClass.AttachToHost(_mainWindow);
			}

			protected override void OnStartup(StartupEventArgs e)
			{
				_mainWindow.Show();
				_testMethod.Invoke(_testClass, new object[0]);
			}
		}
	}
}