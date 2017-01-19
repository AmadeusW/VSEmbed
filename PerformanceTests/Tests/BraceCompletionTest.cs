using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PerformanceTests.Tests
{
	public class BraceCompletionTest : TestBase
	{
		[Params(true, false)]
		public bool Folded { get; set; }

		[Params(10, 100, 1000)]
		public int LineCount { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			var baseText = Snippets.ConsoleApp;
			// Ten repetitions of "a", "b", "c"
			string unfoldedCode = @"void M() { }\r\n";
			string foldedCode = @"void M() { } ";

			string injectedCode = String.Concat(Enumerable.Repeat(Folded ? foldedCode : unfoldedCode, LineCount));

			var testCode = baseText.Insert(Snippets.GetCaretPositionInConsoleApp(Location.AfterClass), injectedCode);
			Host.SetText(testCode);
			Host.MoveCaretToEnd();
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void InvokeBraceCompletion()
		{
			// Brace completion works only for syntax nodes contained directly in affected braces.
			// Remove the namespace's brace and insert it again
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKeystrokes("}");
		}
	}
}
