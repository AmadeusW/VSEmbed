using BenchmarkDotNet.Attributes;
using PerformanceTests.Props;
using System;

namespace PerformanceTests.ExploratoryTests
{
	public class CutCopyPasteUndoTest : TestBase
	{
		[Params(true, false)]
		public bool Undo { get; set; }

		[Params(44, 94, 136, 200)]
		public int LineCount { get; set; }

		public override void SetupHost()
		{
			base.SetupHost();

			Host.SetText(Snippets.ArrayMethods);
			Host.MoveCaret(Snippets.GetCaretPositionInArrayMethods(Location.WithinClass));
		}

		private void Select()
		{
			int times;
			times = LineCount;
			while (times-- > 0) Host.SendKey(System.Windows.Input.Key.Down, System.Windows.Input.ModifierKeys.Shift);
		}

		[Benchmark(Baseline=true, OperationsPerInvoke = 1), STAThread]
		public void Copy()
		{
			Select();
			Host.SendKey(System.Windows.Input.Key.C, System.Windows.Input.ModifierKeys.Control);
		}

		[Benchmark(OperationsPerInvoke = 1), STAThread]
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
