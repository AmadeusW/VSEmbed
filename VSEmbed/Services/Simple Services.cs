﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Internal.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSEmbed.Services
{
	// This file contains services that are more than stubs, but are not very complicated.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member; consult MSDN on the base interfaces.

	///<summary>A WaitDialogFactory that does not show any UI.  Derived classes can inherit WaitDialog to show some UI.</summary>
	public class BaseWaitDialogFactory : IVsThreadedWaitDialogFactory {
		public virtual int CreateInstance(out IVsThreadedWaitDialog2 ppIVsThreadedWaitDialog) {
			ppIVsThreadedWaitDialog = new WaitDialog();
			return 0;
		}
		protected class WaitDialog : IVsThreadedWaitDialog3 {
			// TODO: Actually show some UI
			public virtual void EndWaitDialog(out int pfCanceled) {
				pfCanceled = 0;
			}

			public virtual void HasCanceled(out bool pfCanceled) {
				pfCanceled = false;
			}

			public virtual void StartWaitDialog(string szWaitCaption, string szWaitMessage, string szProgressText, object varStatusBmpAnim, string szStatusBarText, int iDelayToShowDialog, bool fIsCancelable, bool fShowMarqueeProgress) { }

			public virtual void StartWaitDialogWithCallback(string szWaitCaption, string szWaitMessage, string szProgressText, object varStatusBmpAnim, string szStatusBarText, bool fIsCancelable, int iDelayToShowDialog, bool fShowProgress, int iTotalSteps, int iCurrentStep, IVsThreadedWaitDialogCallback pCallback) { }

			public virtual void StartWaitDialogWithPercentageProgress(string szWaitCaption, string szWaitMessage, string szProgressText, object varStatusBmpAnim, string szStatusBarText, bool fIsCancelable, int iDelayToShowDialog, int iTotalSteps, int iCurrentStep) {
			}

			public virtual void UpdateProgress(string szUpdatedWaitMessage, string szProgressText, string szStatusBarText, int iCurrentStep, int iTotalSteps, bool fDisableCancel, out bool pfCanceled) {
				pfCanceled = false;
			}

			int IVsThreadedWaitDialog2.EndWaitDialog(out int pfCanceled) {
				EndWaitDialog(out pfCanceled);
				return 0;
			}

			int IVsThreadedWaitDialog2.HasCanceled(out bool pfCanceled) {
				HasCanceled(out pfCanceled);
				return 0;
			}

			int IVsThreadedWaitDialog2.StartWaitDialog(string szWaitCaption, string szWaitMessage, string szProgressText, object varStatusBmpAnim, string szStatusBarText, int iDelayToShowDialog, bool fIsCancelable, bool fShowMarqueeProgress) {
				StartWaitDialog(szWaitCaption, szWaitMessage, szProgressText, varStatusBmpAnim, szStatusBarText, iDelayToShowDialog, fIsCancelable, fShowMarqueeProgress);
				return 0;
			}

			int IVsThreadedWaitDialog2.StartWaitDialogWithPercentageProgress(string szWaitCaption, string szWaitMessage, string szProgressText, object varStatusBmpAnim, string szStatusBarText, bool fIsCancelable, int iDelayToShowDialog, int iTotalSteps, int iCurrentStep) {
				StartWaitDialogWithPercentageProgress(szWaitCaption, szWaitMessage, szProgressText, varStatusBmpAnim, szStatusBarText, fIsCancelable, iDelayToShowDialog, iTotalSteps, iCurrentStep);
				return 0;
			}

			int IVsThreadedWaitDialog2.UpdateProgress(string szUpdatedWaitMessage, string szProgressText, string szStatusBarText, int iCurrentStep, int iTotalSteps, bool fDisableCancel, out bool pfCanceled) {
				UpdateProgress(szUpdatedWaitMessage, szProgressText, szStatusBarText, iTotalSteps, iCurrentStep, fDisableCancel, out pfCanceled);
				return 0;
			}
		}
	}

#pragma warning disable 0436	// Tell the non-Roslyn compiler to ignore conflicts with inaccessible NoPIA types
	// This class can only be used if VS is set up after the UI thread is created.
	class SyncContextInvoker : IVsInvokerPrivate {
		readonly SynchronizationContext syncContext;
		public SyncContextInvoker(SynchronizationContext syncContext) { this.syncContext = syncContext; }

		public int Invoke([In, MarshalAs(UnmanagedType.Interface)]IVsInvokablePrivate pInvokable) {
			syncContext.Send(o => pInvokable.Invoke(), null);
			return 0;
		}
	}
	class SystemUIHostLocale : IUIHostLocale2 {
		public int GetDialogFont(UIDLGLOGFONT[] pLOGFONT) {
			pLOGFONT[0].lfFaceName = SystemFonts.CaptionFontFamily.Source.Select(c => (ushort)c).ToArray();
			return 0;
		}

		public int GetUILibraryFileName(string lpstrPath, string lpstrDllName, out string pbstrOut) {
			throw new NotImplementedException();
		}

		public int GetUILocale(out uint plcid) {
			throw new NotImplementedException();
		}

		public int LoadDialog(uint hMod, uint dwDlgResId, out IntPtr ppDlgTemplate) {
			throw new NotImplementedException();
		}

		public int LoadUILibrary(string lpstrPath, string lpstrDllName, uint dwExFlags, out uint phinstOut) {
			throw new NotImplementedException();
		}

		public int MungeDialogFont(uint dwSize, byte[] pDlgTemplate, out IntPtr ppDlgTemplateOut) {
			throw new NotImplementedException();
		}
	}
}

namespace Microsoft.Internal.VisualStudio.Shell.Interop
{
	[CompilerGenerated, Guid("20705D94-A39B-4741-B5E1-041C5985EF61"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), TypeIdentifier]
	[ComImport]
	public interface IVsInvokerPrivate {
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		int Invoke([MarshalAs(UnmanagedType.Interface)] [In] IVsInvokablePrivate pInvokable);
	}
	[CompilerGenerated, Guid("20E8B039-A51A-40C6-8F16-2A8BB99E046F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), TypeIdentifier]
	[ComImport]
	public interface IVsInvokablePrivate {
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		int Invoke();
	}
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
