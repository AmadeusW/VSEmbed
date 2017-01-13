using System;
using BenchmarkDotNet.Running;

namespace PerformanceTests
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// SimpleRunner just shows the window with the editor
			//SimpleRunner.Run();

			// DiagnosticRunner runs benchmark code in the UI context
			DiagnosticRunner.Run<BasicTypingTest>(nameof(BasicTypingTest.BasicTypingPerf));

			// BenchmarkRunner runs the benchmark. Run it in Release configuration!
			//var summary = BenchmarkRunner.Run<BasicTypingTest>();
			//Console.ReadLine();
		}
	}
}
