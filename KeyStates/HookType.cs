using System;
using System.Runtime.InteropServices;

namespace KeyStates
{
	internal delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, [In] KBDLLHOOKSTRUCT lParam);

	internal enum HookType
	{
		// ReSharper disable InconsistentNaming
		WH_JOURNALRECORD = 0,
		WH_JOURNALPLAYBACK = 1,
		WH_KEYBOARD = 2,
		WH_GETMESSAGE = 3,
		WH_CALLWNDPROC = 4,
		WH_CBT = 5,
		WH_SYSMSGFILTER = 6,
		WH_MOUSE = 7,
		WH_HARDWARE = 8,
		WH_DEBUG = 9,
		WH_SHELL = 10,
		WH_FOREGROUNDIDLE = 11,
		WH_CALLWNDPROCRET = 12,
		WH_KEYBOARD_LL = 13,
		WH_MOUSE_LL = 14
		// ReSharper restore InconsistentNaming
	}

	// ReSharper disable InconsistentNaming
	[StructLayout(LayoutKind.Sequential)]
	public class KBDLLHOOKSTRUCT
	{
		public uint vkCode;
		public uint scanCode;
		public KBDLLHOOKSTRUCTFlags flags;
		public uint time;
		public UIntPtr dwExtraInfo;
	}
	// ReSharper enable InconsistentNaming

	// ReSharper disable InconsistentNaming
	[Flags]
	public enum KBDLLHOOKSTRUCTFlags : uint

	{
		LLKHF_EXTENDED = 0x01,
		LLKHF_INJECTED = 0x10,
		LLKHF_ALTDOWN = 0x20,
		LLKHF_UP = 0x80,
	}
	// ReSharper restore InconsistentNaming
}
