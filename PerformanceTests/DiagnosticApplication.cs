﻿using System;
using System.Linq;
using System.Threading;
using System.Windows;
using VSEmbed;
using BenchmarkDotNet.Attributes;
using System.Reflection;
using VSEmbed.Contracts;
using PerformanceTests.Tests;

namespace PerformanceTests
{
	/// <summary>
	/// Creates a window with the VS editor, runs benchmark code and allows inspection in the UI.
	/// </summary>
	internal class DiagnosticApplication : Application
	{
		private readonly IEmbeddedTextViewHost _mainWindow;
		private readonly TestBase _testClass;
		private readonly Action _testMethod;

		/// <summary>
		/// Runs benchmark and allows its inspection in the GUI
		/// </summary>
		/// <param name="testClass">Instace of TestBase. You may customize parameters.</param>
		/// <param name="testMethod">Optionally, a test method to run</param>
		internal static void Run(TestBase testClass, Action testMethod = null)
		{
			if (testClass == null)
				throw new ArgumentNullException("To properly initialize MEF, please provide an instance of TestBase");

			// The console app will remain active until DiagnosticApplication's window is closed
			new DiagnosticApplication(testClass, testMethod).Run();
		}

		private DiagnosticApplication(TestBase testClass, Action testMethod)
		{
			this._testClass = testClass;
			this._testMethod = testMethod;
			_mainWindow = new VSEmbed.DemoApp.MainWindow();
			_testClass.AttachToHost(_mainWindow);
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			_mainWindow.Show();
			_testMethod?.Invoke();
		}
	}
}