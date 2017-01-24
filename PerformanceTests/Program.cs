using System;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;

namespace PerformanceTests
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			//UITest();
			ExploratoryBenchmark();
		}

		/// <summary>
		/// BenchmarkRunner runs the benchmark. Run it in Release configuration!
		/// </summary>
		private static void ExploratoryBenchmark()
		{
			var config = ManualConfig.Create(DefaultConfig.Instance);
			config.Add(new Job(DateTime.Now.ToString("yyyyMMdd") + " - exploratory")
			{
				Run = { LaunchCount = 3, TargetCount = 1, WarmupCount = 1, UnrollFactor = 1, InvocationCount = 1 }
			});
			BenchmarkRunner.Run<ExploratoryTests.BasicTypingTest>(config);
			BenchmarkRunner.Run<ExploratoryTests.CompletionTest>(config);
			BenchmarkRunner.Run<ExploratoryTests.CutCopyPasteUndoTest>(config);
			BenchmarkRunner.Run<ExploratoryTests.AutoformattingBlockTest>(config);
			BenchmarkRunner.Run<ExploratoryTests.AutoformattingNewlineTest>(config);
			Console.ReadLine();
		}

		/// <summary>
		/// BenchmarkRunner runs the benchmark. Run it in Release configuration!
		/// </summary>
		private static void FocusedBenchmark()
		{
			var config = ManualConfig.Create(DefaultConfig.Instance);
			config.Add(new Job(DateTime.Now.ToString("yyyyMMdd") + " - focused")
			{
				Run = { LaunchCount = 3, TargetCount = 1, WarmupCount = 1, UnrollFactor = 4, InvocationCount = 4 }
			});
			BenchmarkRunner.Run<ExploratoryTests.CompletionTest>(config);
			Console.ReadLine();
		}

		/// <summary>
		/// DiagnosticRunner runs benchmark code in the UI context
		/// </summary>
		private static void UITest()
		{
			var test = new ExploratoryTests.BasicTypingTest()
			{
				CurrentContentType = ContentType.CSharp,
				ClassCount = 2,
				LargeFile = true
			};
			// DiagnosticRunner runs benchmark code in the UI context
			DiagnosticApplication.Run(test, test.TypingBasic);
		}
	}
}
