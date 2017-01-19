using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.Tests
{
	public class BasicTypingTest : TestBase
	{
		[Params(1, 10)]
		public int ClassCount { get; set; }

		[Params(true, false)]
		public bool CommentedOut { get; set; }

		[Params(true, false)]
		public bool LargeFile { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			Host.SetText(Snippets.ConsoleApp + (LargeFile ? Snippets.ExtraCode : String.Empty));
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(Location.AfterClass));
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void BasicTypingPerf()
		{
			// Tests performance of working within /* block comments */
			Host.SendKeystrokes(CommentedOut ? "/*" : "  " );

			// Tests basic typing scenario with special characters
			for (int c = 0; c < ClassCount; c++)
			{
				Host.SendKeystrokes("class  SampleClass" + c); // Double space dismisses intellisense
				Host.SendKeystrokes(@"
{
private
 int  SampleMethod()
{
int  x = 5;
int  y = 6;
int  z = x ++ y;;
return  z ** 1234567890;
}
}

");
			}

			Host.SendKeystrokes(CommentedOut ? "*/" : "  ");
		}
	}
}
