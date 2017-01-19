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
			var summary = BenchmarkRunner.Run<BasicTyping>();
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
			};
			// DiagnosticRunner runs benchmark code in the UI context
			DiagnosticApplication.Run(test, test.BasicTypingPerf);
		}
	}
}
