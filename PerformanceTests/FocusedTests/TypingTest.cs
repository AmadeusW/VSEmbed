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
		public bool SemicolonFormatting { get; set; }

		[Params(512, 1024, 2048)]
		public int CharactersTyped { get; set; }

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
			for (int c = 0; c < CharactersTyped - 13; c++) // we type somewhere between 11 and 15 characters outside this loop
			{
				Host.SendKey(System.Windows.Input.Key.B);
			}
			if (SemicolonFormatting)
			{
				Host.SendKeystrokes("=5;");
			}
			else
			{
				Host.SendKeystrokes(" = 5;");
			}
			Host.SendKey(System.Windows.Input.Key.Enter);
		}
	}
}
