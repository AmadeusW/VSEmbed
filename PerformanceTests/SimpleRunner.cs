using System;
using System.Threading;
using System.Windows;

namespace PerformanceTests
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
				var test = new BasicTypingTest() as IDebuggableTest;
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