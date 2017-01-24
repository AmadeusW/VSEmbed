using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.ExploratoryTests
{
	public class BasicTypingTest : TestBase
	{
		[Params(8, 16, 32)]
		public int ClassCount { get; set; }

		[Params(true, false)]
		public bool LargeFile { get; set; }

		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		protected override void SetupHost()
		{
			Host.SetContentType(CurrentContentType.ToString());

			Host.SetText(Snippets.ConsoleApp + (LargeFile ? Snippets.ArrayMethods : String.Empty));
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(Location.AfterClass));
		}

		[Benchmark(Baseline = true, OperationsPerInvoke = 1), STAThread]
		public void TypingCommentedOut()
		{
			Host.SendKeystrokes("/*");

			for (int c = 0; c < ClassCount; c++)
			{
				Host.SendKeystrokes("class SampleClass" + c); // Double space dismisses intellisense
				Host.SendKeystrokes(@"
{
private int SampleMethod()
{
int x = 5;
int y = 6;
int z = x + y;
return z * 1234567890;
}
}

");
			}

			Host.SendKeystrokes("*/");
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void TypingBasic()
		{
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
int  z = x
 + y;;
return  z
 * 1234567890;
}
}

");
			}
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void TypingWihtoutCompletion()
		{
			// We substitute Escape key for ~
			for (int c = 0; c < ClassCount; c++)
			{
				Host.SendKeystrokes("c~lass S~ampleClass" + c); // Double space dismisses intellisense
				Host.SendKeystrokes(@"
{
p~rivate i~nt S~ampleMethod()
{
i~nt x = 5;
i~nt y = 6;
i~nt z = x~ + y~;
r~eturn z~ * 1234567890;
}
}

");
			}
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void TypingWithEagerCompletion()
		{
			// Spacebar accepts intellisense suggestion
			for (int c = 0; c < ClassCount; c++)
			{
				Host.SendKeystrokes("c  SampleClass" + c); // Double space dismisses intellisense
				Host.SendKeystrokes(@"
{
pr  i  SampleMethod()
{
i  x = 5;
i  y = 6;
i  z = x
+  y
;
ret  z
* 1234567890;
}
}

");
			}
		}
	}
}
