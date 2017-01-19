using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;
using System.Text.RegularExpressions;

namespace PerformanceTests.Tests
{
	public class BraceCompletionTest : TestBase
	{
		[Params(true, false)]
		public bool Folded { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			// Remove comments
			var testCode = Regex.Replace(Snippets.ExtraCode, @"\/\/(.*?)\r?\n", String.Empty);
			// Conditionally remove newlines
			if (Folded)
				testCode = testCode.Replace("\r\n", String.Empty);

			Host.SetText(Snippets.ConsoleApp + testCode);
			Host.MoveCaretToEnd();
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void InvokeBraceCompletion()
		{
			// Brace completion works only for syntax nodes contained directly in affected braces.
			//Host.SendKey(System.Windows.Input.Key.Enter);
			Host.SendKey(System.Windows.Input.Key.X);
		}
	}
}
