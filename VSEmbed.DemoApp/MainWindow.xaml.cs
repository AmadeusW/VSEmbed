﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

		public void SetContentType(string contentType)
		{
			this.mainTextViewHost.ContentType = contentType;
		}
	}
}
