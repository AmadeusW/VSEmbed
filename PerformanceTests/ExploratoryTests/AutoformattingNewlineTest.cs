using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PerformanceTests.ExploratoryTests
{
	public class AutoformattingNewlineTest : TestBase
	{
		[Params(true, false)]
		public bool NeedToFormat { get; set; }

		[Params(true, false)]
		public bool NeedToIndent { get; set; }

		/// <summary>
		/// Number of elements to autoformat
		/// </summary>
		[Params(128, 256, 512, 1024)]
		public int ElementCount { get; set; }

		[Params(128, 256, 512)]
		public int LineCount { get; set; }

		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		public override void SetupHost()
		{
			Host.SetContentType(CurrentContentType.ToString());

			var baseText = Snippets.ConsoleApp;
			// Ten repetitions of "a", "b", "c"
			string injectedFormatted =
				"\r\n"
				+ (NeedToIndent ? String.Empty : "            ")
				+ "Main (new string[] {"
				+ Enumerable.Repeat(@" ""x"",", ElementCount)
				+ @" ""x""})";
			string injectedUnformatted = 
				"\r\n" 
				+ (NeedToIndent ? String.Empty : "            ")
				+ "Main  (  new   string  [  ]   {"
				+ Enumerable.Repeat(@"   ""x""   ,", ElementCount)
				+ @"    ""x""   }   )   ";

			string injectedCode = String.Concat(Enumerable.Repeat(NeedToFormat ? injectedUnformatted : injectedFormatted, ElementCount));

			var testCode = baseText.Insert(Snippets.GetCaretPositionInConsoleApp(Location.WithinMethod), injectedCode);
			Host.SetText(testCode);
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(Location.WithinMethod) + 2); // Advance to next line
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void AutoFormat()
		{
			for (int i = 0; i < ElementCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.End);
				Host.SendKey(System.Windows.Input.Key.OemSemicolon);
				Host.SendKey(System.Windows.Input.Key.Down);
			}
		}
	}
}
