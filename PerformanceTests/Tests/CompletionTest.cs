using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.Tests
{
	public class CompletionTest : TestBase
	{
		[Params(0, 10)]
		public int IntellisenseLaunchCount { get; set; }

		[Params(Location.OutsideNamespace, Location.WithinClass, Location.WithinMethod)]
		public Location CompletionLocation { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			Host.SetText(Snippets.ConsoleApp);
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(CompletionLocation));
		}

		[Benchmark, STAThread]
		public void LaunchIntellisense()
		{
			// Test: Launch intellisense
			for (int i = 0; i < IntellisenseLaunchCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.Space, System.Windows.Input.ModifierKeys.Control);
				Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
			}
		}
	}
}
