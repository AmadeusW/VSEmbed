using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace VSEmbed
{
	///<summary>Sets up assembly redirection to load Visual Studio assemblies.</summary>
	///<remarks>This class must be initialized before anything else is JITted.</remarks>
	public static class VsLoader
	{
		/// <summary>
		/// A list of key names for versions of Visual Studio which have the editor components 
		/// necessary to create an EditorHost instance.  Listed in preference order.
		/// Stolen from @JaredPar
		/// </summary>
		private static readonly string[] SkuKeyNames = {
			"VisualStudio",	// Standard non-express SKU of Visual Studio
			"WDExpress",	// Windows Desktop express
			"VCSExpress",	// Visual C# express
			"VCExpress",	// Visual C++ express
			"VBExpress",	// Visual Basic Express
		};

		///<summary>Gets the installation directory for the specified version.</summary>
		private static string GetInstallationDirectory(Version version)
		{
			return SkuKeyNames.Select(sku =>
				Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\" + sku + @"\" + version.ToString(2), "InstallDir", null) as string
			).FirstOrDefault(p => p != null);
		}

		///<summary>Initializes the assembly loader with the specified version of Visual Studio.</summary>
		public static void Load(Version vsVersion)
		{
			if (VsVersion != null)
				throw new InvalidOperationException("VsLoader cannot be initialized twice");
			if (string.IsNullOrEmpty(GetInstallationDirectory(vsVersion)) || !Directory.Exists(GetInstallationDirectory(vsVersion)))
				throw new ArgumentException("Cannot locate Visual Studio v" + vsVersion);

			VsVersion = vsVersion;
			InstallationDirectory = GetInstallationDirectory(VsVersion);
		}

		///<summary>Gets the version of Visual Studio that will be loaded.  This cannot be changed, because the CLR caches assembly loads.</summary>
		public static Version VsVersion { get; private set; }

		///<summary>Gets the installation directory for the loaded VS version.</summary>
		public static string InstallationDirectory { get; private set; }
	}
}
