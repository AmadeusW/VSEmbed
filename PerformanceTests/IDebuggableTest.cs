﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VSEmbed.Contracts;

namespace PerformanceTests
{
	interface IDebuggableTest
	{
		void Setup();
		void AttachToHost(IEmbeddedTextViewHost host);
	}
}
