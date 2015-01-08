using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyStates
{
	public static class PassiveKeyboardMonitor
	{
		private static readonly LowLevelKeyboardProc CallbackHookProc;
		private static IntPtr _hookPtr = IntPtr.Zero;

		#region Events

		public static KeyEvent KeyDown;
		public static KeyEvent KeyUp;
		public static KeyEvent KeyPressed;

		#endregion

		static PassiveKeyboardMonitor()
		{
			CallbackHookProc = CallbackFunction;
		}

		#region Methods

		public static void Start()
		{
			//Timer.Start();
			using (var process = Process.GetCurrentProcess())
			using (var module = process.MainModule)
			{
				var hModule =  NativeMethods.GetModuleHandle(module.ModuleName);
				_hookPtr = NativeMethods.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, CallbackHookProc, hModule, 0);

				if (_hookPtr == IntPtr.Zero)
					throw new KeyboardMonitorException("Hook failed to start.");
			}
		}

		public static void Stop()
		{

			if (_hookPtr != IntPtr.Zero && NativeMethods.UnhookWindowsHookEx(_hookPtr))
				_hookPtr = IntPtr.Zero;
			else
				throw new KeyboardMonitorException("Hook failed to stop.");
		}

		#endregion

		private static IntPtr CallbackFunction(int nCode, IntPtr wParam, [In] KBDLLHOOKSTRUCT lParam)
		{
			if (nCode < 0)
			{
				return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
			}

			var keyPressed = (VirtualKeyCode)lParam.vkCode;

			if ((lParam.flags & KBDLLHOOKSTRUCTFlags.LLKHF_UP) == KBDLLHOOKSTRUCTFlags.LLKHF_UP)
			{
				KeyUp?.Invoke(new KeyEventArgs(keyPressed));
			}
			else
			{
				KeyDown?.Invoke(new KeyEventArgs(keyPressed));
			}

			return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
		}
    }
}
