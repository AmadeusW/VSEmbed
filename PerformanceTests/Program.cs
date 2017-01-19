using System;
using PerformanceTests.Tests;
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
			UITest();
			//Benchmark();
		}

		/// <summary>
		/// BenchmarkRunner runs the benchmark. Run it in Release configuration!
		/// </summary>
		private static void Benchmark()
		{
			var config = ManualConfig.Create(DefaultConfig.Instance);
			config.Add(new Job("TestJob")
			{
				Run = { LaunchCount = 4, TargetCount = 1, WarmupCount = 1, UnrollFactor = 1 }
			});
			var summary = BenchmarkRunner.Run<BasicTyping>(config);
			Console.ReadLine();
		}

		/// <summary>
		/// DiagnosticRunner runs benchmark code in the UI context
		/// </summary>
		private static void UITest()
		{
			var test = new BasicTyping()
			{
				CurrentContentType = ContentType.CSharp,
				ClassCount = 2,
				CommentedOut = false,
				LargeFile = true
			};
			// DiagnosticRunner runs benchmark code in the UI context
			DiagnosticApplication.Run(test, test.BasicTypingPerf);
		}
	}
}
