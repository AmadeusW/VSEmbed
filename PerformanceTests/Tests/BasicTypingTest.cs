using BenchmarkDotNet.Attributes;
using System;

namespace PerformanceTests.Tests
{
	public class BasicTyping : TestBase
	{
		[Params(1, 10)]
		public int ClassCount { get; set; }

		public bool Clear { get; set; } = true;

		[Benchmark, STAThread]
		public void BasicTypingPerf()
		{
			Host.SendKeystrokes("namespace");
			Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
			Host.SendKeystrokes(" TestNamespace{\r\n");
			for (int i = 0; i < ClassCount; i++)
			{
				Host.SendKeystrokes(
@"class  SampleClass
{
private
 int");
				Host.SendKey(System.Windows.Input.Key.Escape); // Dismiss intellisense
				Host.SendKeystrokes(" SampleMethod" + i + "()");
				Host.SendKeystrokes(@"
{
int  x = 5;
int  y = 6;
int  z = x+y;
return  z*1234567890;
}
}

");
			}
			Host.SendKeystrokes("}");

			if (Clear)
				Host.ClearEditor();
		}
	}
}
