using BenchmarkDotNet.Attributes;
using System;

namespace PerformanceTests.Tests
{
	public class BasicTyping : TestBase
	{
		[Params(1, 10)]
		public int ClassCount { get; set; }

		[Params(0, 1, 10)]
		public int IntellisenseLaunchCount { get; set; }

		[Benchmark, STAThread]
		public void BasicTypingPerf()
		{
			Host.SendKeystrokes("namespace");
			Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
			Host.SendKeystrokes(" TestNamespace{\r\n");

			// Test: Type code
			for (int c = 0; c < ClassCount; c++)
			{
				Host.SendKeystrokes("class  SampleClass" + c);
Host.SendKeystrokes(@"
{
private
 int");
				Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
				Host.SendKeystrokes(@"  SampleMethod()
{
int  x = 5;
int  y = ");

				Host.SendKeystrokes(@"6;
int  z = x ++ y;;
return  z ** 1234567890;
}
}

");
			}

			// Test: Launch intellisense
			for (int i = 0; i < IntellisenseLaunchCount; i++)
			{
				Host.SendKey(System.Windows.Input.Key.Space, System.Windows.Input.ModifierKeys.Control);
				Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
			}

			Host.SendKeystrokes("}");

			if (Clear)
				Host.ClearEditor();
		}
	}
}
