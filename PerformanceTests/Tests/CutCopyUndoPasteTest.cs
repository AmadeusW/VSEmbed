using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.Tests
{
	public class CutCopyPasteUndoTest : TestBase
	{
		[Params(true, false)]
		public bool LargeFile { get; set; }

		[Params(true, false)]
		public bool Undo { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			Host.SetText(Snippets.ConsoleApp + (LargeFile ? Snippets.ExtraCode : String.Empty));
			Host.MoveCaret(Snippets.GetCaretPositionInConsoleApp(Location.AfterClass));
		}

		private void Select()
		{
			int times;
			times = 7;
			while (--times > 0) Host.SendKey(System.Windows.Input.Key.Up, System.Windows.Input.ModifierKeys.Shift);
		}

		[Benchmark(Baseline=true, OperationsPerInvoke = 1), STAThread]
		public void Copy()
		{
			Select();
			Host.SendKey(System.Windows.Input.Key.C, System.Windows.Input.ModifierKeys.Control);
		}

		[Benchmark(OperationsPerInvoke = 2), STAThread]
		public void CopyPaste()
		{
			Select();
			Host.SendKey(System.Windows.Input.Key.C, System.Windows.Input.ModifierKeys.Control);
			Host.SendKey(System.Windows.Input.Key.V, System.Windows.Input.ModifierKeys.Control);
			if (Undo) Host.SendKey(System.Windows.Input.Key.Z, System.Windows.Input.ModifierKeys.Control);
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void Cut()
		{
			Select();
			Host.SendKey(System.Windows.Input.Key.X, System.Windows.Input.ModifierKeys.Control);
			if (Undo) Host.SendKey(System.Windows.Input.Key.Z, System.Windows.Input.ModifierKeys.Control);
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
		public void CutPaste()
		{
			Select();
			Host.SendKey(System.Windows.Input.Key.X, System.Windows.Input.ModifierKeys.Control);
			Host.SendKey(System.Windows.Input.Key.V, System.Windows.Input.ModifierKeys.Control);
			if (Undo) Host.SendKey(System.Windows.Input.Key.Z, System.Windows.Input.ModifierKeys.Control);
		}
	}
}
