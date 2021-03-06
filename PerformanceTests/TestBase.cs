﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSEmbed;

namespace PerformanceTests
{
	public enum ContentType { text, CSharp }

	public abstract class TestBase
	{
		protected VSEmbed.DemoApp.EditorWindow Host { get; private set; }
		protected abstract void SetupHost();

		static TestBase()
		{
			VsServiceProvider.Initialize();
			VsMefContainerBuilder.CreateDefault().Build();
			initializeRoslynForegroundThreadDataObject();
		}

		private static void initializeRoslynForegroundThreadDataObject()
		{
			var assembly = Assembly.Load("Microsoft.CodeAnalysis.EditorFeatures");
			var t_foregroundThreadData = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadData");
			var m_createDefault = t_foregroundThreadData.GetMethod("CreateDefault", BindingFlags.Static | BindingFlags.NonPublic);
			int foregroundThreadDataKind = 4;
			var result = m_createDefault.Invoke(null, new object[] { foregroundThreadDataKind });
			var t_foregroundThreadAffinitizedObject = assembly.GetType("Microsoft.CodeAnalysis.Editor.Shared.Utilities.ForegroundThreadAffinitizedObject");
			var m_currentForegroundThreadData = t_foregroundThreadAffinitizedObject.GetProperty("CurrentForegroundThreadData", BindingFlags.Static | BindingFlags.NonPublic);
			m_currentForegroundThreadData.SetValue(null, result);
		}	

		[Setup]
		public virtual void Setup()
		{
			Host = new VSEmbed.DemoApp.EditorWindow();
			Host.Show();
			SetupHost();
		}

		[Cleanup]
		public void Cleanup()
		{
			Host.ClearText();
			Host.Close();
			Host = null;

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		/// <summary>
		/// Sets up the host window. 
		/// This method is used only in UI debugging of the test,
		/// where we provide our own window.
		/// Benchmark needs to use the Setup method.
		/// </summary>
		/// <param name="host"></param>
		internal void AttachToHost(VSEmbed.DemoApp.EditorWindow host)
		{
			Host = host;
			Host.Show();
			SetupHost();
		}
	}
}
