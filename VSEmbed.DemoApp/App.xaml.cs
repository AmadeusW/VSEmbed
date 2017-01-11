using System;
using System.Windows;

namespace VSEmbed.DemoApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public App() {
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
		}
	}
}
