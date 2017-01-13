using System;
using PerformanceTests.Tests;
using BenchmarkDotNet.Running;

namespace PerformanceTests
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// DiagnosticRunner runs benchmark code in the UI context
			var test = new BasicTyping()
			{
				CurrentContentType = ContentType.CSharp
			};
			DiagnosticApplication.Run(test, test.BasicTypingPerf);

			// BenchmarkRunner runs the benchmark. Run it in Release configuration!
			//var summary = BenchmarkRunner.Run<BasicTyping>();
			//Console.ReadLine();
		}
	}
}
