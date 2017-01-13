using System;
using System.Linq;
using System.Threading;
using System.Windows;
using VSEmbed;
using BenchmarkDotNet.Attributes;
using System.Reflection;
using VSEmbed.Contracts;

namespace PerformanceTests.Runners
{
	/// <summary>
	/// Creates a window with the VS editor and runs benchmark code.
	/// </summary>
	internal class DiagnosticRunner
	{
		/// <summary>
		/// Runs a specific benchmark method and displays its execution in the UI
		/// </summary>
		/// <typeparam name="T">Benchmark test type</typeparam>
		/// <param name="testName">Name of the benchmark method to run</param>
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