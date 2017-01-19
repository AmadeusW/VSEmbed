﻿using System;
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
				Run = { LaunchCount = 3, TargetCount = 1, WarmupCount = 1, UnrollFactor = 1, InvocationCount = 1 }
			});
			var summary1 = BenchmarkRunner.Run<CompletionTest>(config);
			var summary2 = BenchmarkRunner.Run<CutCopyPasteUndoTest>(config);
			var summary3 = BenchmarkRunner.Run<BasicTypingTest>(config);
			Console.ReadLine();
		}

		/// <summary>
		/// DiagnosticRunner runs benchmark code in the UI context
		/// </summary>
		private static void UITest()
		{
			var test = new CompletionTest()
			{
				CurrentContentType = ContentType.CSharp,
				IntellisenseLaunchCount = 2,
				LargeFile = true,
				CompletionLocation = Props.Location.WithinMethod,
			};
			// DiagnosticRunner runs benchmark code in the UI context
			DiagnosticApplication.Run(test, test.LaunchIntellisenseAndComplete);
		}
	}
}
