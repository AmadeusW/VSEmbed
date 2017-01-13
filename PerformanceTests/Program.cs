using System;
using BenchmarkDotNet.Running;

namespace PerformanceTests
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
#if true
			//DiagnosticRunner.Run<BasicTypingTest>(nameof(BasicTypingTest.BasicTypingPerf));
			SimpleRunner.Run();
#else
			var summary = BenchmarkRunner.Run<BasicTypingTest>();
			Console.ReadLine();
#endif
		}
	}
}
