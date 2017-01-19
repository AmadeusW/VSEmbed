using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.Tests
{
	public class CompletionTest : TestBase
	{
		[Params(10, 100)]
		public int IntellisenseLaunchCount { get; set; }

		[Params(true, false)]
		public bool LargeFile { get; set; }

		[Params(Location.OutsideNamespace, Location.WithinClass, Location.WithinMethod)]
		public Location CompletionLocation { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			Host.SetText(Snippets.ConsoleApp + (LargeFile ? Snippets.ExtraCode : String.Empty));
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(CompletionLocation));
		}

		[Benchmark(Baseline = true, OperationsPerInvoke = 1), STAThread]
		public void LaunchIntellisense()
		{
			Host.SendKey(System.Windows.Input.Key.Space);
			// Test: Launch intellisense
			for (int i = 0; i < IntellisenseLaunchCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.Space, System.Windows.Input.ModifierKeys.Control);
				Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
			}
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void LaunchIntellisenseAndComplete()
		{
			Host.SendKey(System.Windows.Input.Key.S);
			for (int i = 0; i < IntellisenseLaunchCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.Space, System.Windows.Input.ModifierKeys.Control);
				Host.SendKey(System.Windows.Input.Key.Enter); // Complete
			}
		}
	}
}
