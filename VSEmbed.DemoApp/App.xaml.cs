using System;
using System.Windows;

namespace VSEmbed.DemoApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		public App() {
			VsLoader.Load(new Version(14, 0, 0, 0));
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
		}
	}
}
