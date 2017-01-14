using BenchmarkDotNet.Attributes;
using System;

namespace PerformanceTests.Tests
{
	public class BasicTyping : TestBase
	{
		[Benchmark, STAThread]
		public void BasicTypingPerf()
		{
			Host.SendKeystrokes("namespace");
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
			Host.SendKey(System.Windows.Input.Key.Back);
		}
	}
}
