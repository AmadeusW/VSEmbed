using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VSEmbed.DemoApp {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
		}

		public void SendKeyInput()
		{
			var cp = this.mainTextViewHost as ContentPresenter;
			var content = cp.Content as ContentControl;
			var grid = content.Content as System.Windows.Controls.Grid;
			var wpfTextView = grid.Children[0];
			System.Windows.Input.Test.SendKeys.Send(wpfTextView, "namespace");
			//System.Windows.Input.Test.SendKeys.Send(wpfTextView, Key.A, ModifierKeys.None);
			//System.Windows.Input.Test.SendKeys.Send(wpfTextView, Key.B, ModifierKeys.None);
		}
	}
}
