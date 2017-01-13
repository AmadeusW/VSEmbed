using System;
using System.Threading;
using System.Windows;
using PerformanceTests.Tests;

namespace PerformanceTests.Runners
{
	/// <summary>
	/// Shows a window with the VS editor.
	/// </summary>
	internal class SimpleRunner
	{
		/// <summary>
		/// Shows a window with the VS editor.
		/// </summary>
		internal static void Run()
		{
			var thread = new Thread(() =>
			{
				// We need to create an instance of TestBase to initialize MEF and language service
				var test = new BasicTyping() as IDebuggableTest;
				test.AttachToHost(null);
				new WpfApplication().Run();
			});

			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}

		private class WpfApplication : Application
		{
			private readonly VSEmbed.DemoApp.MainWindow _mainWindow;

			public WpfApplication()
			{
				_mainWindow = new VSEmbed.DemoApp.MainWindow();
			}

			protected override void OnStartup(StartupEventArgs e)
			{
				_mainWindow.Show();
			}
		}
	}
}