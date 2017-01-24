using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PerformanceTests.ExploratoryTests
{
	/// <summary>
	/// Finds how much time it takes to autoformat a concatenation of elements, e.g.
	/// { void M() { } void M() { } void M() { } void M() { } void M() { } }
	/// into multiple lines
	/// </summary>
	public class AutoformattingBlockTest : TestBase
	{
		[Params(true, false)]
		public bool Baseline { get; set; }

		[Params(128, 256, 512, 1024, 2048)]
		public int LineCount { get; set; }

		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		protected override void SetupHost()
		{
			Host.SetContentType(CurrentContentType.ToString());

			var baseText = Snippets.ConsoleApp;

			string properCode = "void M() { }\r\n";
			string concatenatedCode = "void M() { } ";

			string injectedCode = String.Concat(Enumerable.Repeat(Baseline ? properCode : concatenatedCode, LineCount));

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
