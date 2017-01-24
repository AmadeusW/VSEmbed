using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfSendKeys;

namespace VSEmbed.DemoApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class EditorWindow : Window
	{
		UIElement _wpfTextView;
		public EditorWindow()
		{
			InitializeComponent();

			var cp = this.mainTextViewHost as ContentPresenter;
			var content = cp.Content as ContentControl;
			var grid = content.Content as System.Windows.Controls.Grid;
			_wpfTextView = grid.Children[0];
		}

		public void SendKeystrokes(string input)
			=> SendKeys.Send(_wpfTextView, input);

		public void SendKey(Key key, ModifierKeys modifiers = ModifierKeys.None)
			=> SendKeys.Send(_wpfTextView, new KeyPressInfo(key, modifiers));

		public void SetContentType(string contentType)
			=> this.mainTextViewHost.ContentType = contentType;

		public void ClearText()
			=> this.mainTextViewHost.Clear();

		public void SetText(string text)
			=> this.mainTextViewHost.SetText(text);

		public void MoveCaret(int position)
			=> this.mainTextViewHost.MoveCaret(position);

		public void MoveCaretToEnd()
			=> this.mainTextViewHost.MoveCaretToEnd();
	}
}
