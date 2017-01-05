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

namespace VSEmbed.DemoApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		UIElement _wpfTextView;
		public MainWindow()
		{
			InitializeComponent();

			var cp = this.mainTextViewHost as ContentPresenter;
			var content = cp.Content as ContentControl;
			var grid = content.Content as System.Windows.Controls.Grid;
			_wpfTextView = grid.Children[0];
		}

		public void SendKeystrokes(string input)
		{
			System.Windows.Input.Test.SendKeys.Send(_wpfTextView, input);
		}

		public void SendKey(Key key, ModifierKeys modifiers = ModifierKeys.None)
		{
			System.Windows.Input.Test.SendKeys.Send(_wpfTextView, key, modifiers);
		}
	}
}
