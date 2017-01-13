using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VSEmbed.Contracts;

namespace VSEmbed.DemoApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IEmbeddedTextViewHost
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

		void IEmbeddedTextViewHost.SendKeystrokes(string input)
			=> System.Windows.Input.Test.SendKeys.Send(_wpfTextView, input);

		void IEmbeddedTextViewHost.SendKey(Key key, ModifierKeys modifiers)
			=> System.Windows.Input.Test.SendKeys.Send(_wpfTextView, key, modifiers);

		void IEmbeddedTextViewHost.SetContentType(string contentType)
			=> this.mainTextViewHost.ContentType = contentType;

		void IEmbeddedTextViewHost.Show() 
			=> this.Show();

		void IEmbeddedTextViewHost.Close() 
			=> this.Close();
	}
}
