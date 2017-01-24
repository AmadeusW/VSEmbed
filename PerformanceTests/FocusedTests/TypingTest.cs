using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.FocusedTests
{
	public class TypingTest : TestBase
	{
		[Params(true, false)]
		public bool Comment { get; set; }

		[Params(true, false)]
		public bool Formatting { get; set; }

		[Params(ContentType.text, ContentType.CSharp)]
		public ContentType CurrentContentType { get; set; }

		protected override void SetupHost()
		{
			Host.SetContentType(CurrentContentType.ToString());

			Host.SetText(Snippets.ConsoleApp);
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(Location.WithinMethod));
			if (Comment)
				Host.SendKeystrokes("//");
		}

		[Benchmark, STAThread]
		public void Typing()
		{
			Host.SendKeystrokes("v~ar  a~");
			for (int c = 0; c < 1000; c++)
			{
				Host.SendKey(System.Windows.Input.Key.B);
			}
		}
	}
}
