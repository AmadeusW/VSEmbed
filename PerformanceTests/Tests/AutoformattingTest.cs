using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PerformanceTests.Tests
{
	public class AutoformattingTest : TestBase
	{
		[Params(true, false)]
		public bool NeedToFormat { get; set; }

		[Params(true, false)]
		public bool NeedToIndent { get; set; }

		[Params(10, 100)]
		public int LineCount { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			var baseText = Snippets.ConsoleApp;
			// Ten repetitions of "a", "b", "c"
			string injectedFormatted =
				"\r\n"
				+ (NeedToIndent ? String.Empty : "            ")
				+ @"Main (new string[] { ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"", ""a"", ""b"", ""c"" })";
			string injectedUnformatted = 
				"\r\n" 
				+ (NeedToIndent ? String.Empty : "            ")
				+ @"Main  (  new   string  [    ]    {  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  ,  ""a""  ,  ""b""  ,  ""c""  }   ) ";

			string injectedCode = String.Concat(Enumerable.Repeat(NeedToFormat ? injectedUnformatted : injectedFormatted, LineCount));

			var testCode = baseText.Insert(224, injectedCode);
			Host.SetText(testCode);
			Host.MoveCaret(226);
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void AutoFormat()
		{
			for (int i = 0; i < LineCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.End);
				Host.SendKey(System.Windows.Input.Key.OemSemicolon);
				Host.SendKey(System.Windows.Input.Key.Down);
			}
		}
	}
}
