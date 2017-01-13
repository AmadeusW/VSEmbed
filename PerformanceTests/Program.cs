using System;
using PerformanceTests.Runners;
using PerformanceTests.Tests;
using BenchmarkDotNet.Running;

namespace PerformanceTests
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// SimpleRunner just shows the window with the editor
			SimpleRunner.Run();

			// DiagnosticRunner runs benchmark code in the UI context
			//DiagnosticRunner.Run<BasicTyping>(nameof(BasicTyping.BasicTypingPerf));

			// BenchmarkRunner runs the benchmark. Run it in Release configuration!
			//var summary = BenchmarkRunner.Run<BasicTyping>();
			//Console.ReadLine();
		}
	}
}
