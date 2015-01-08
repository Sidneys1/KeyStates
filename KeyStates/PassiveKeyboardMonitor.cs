using System;

namespace KeyStates
{
	public class PassiveKeyboardMonitor
	{
		private readonly IntPtr _hookHandle;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly KeyboardHookProc _hookProc;

		public event KeyEvent KeyDown;
		public event KeyEvent KeyUp;

		public PassiveKeyboardMonitor()
		{
			var hInstance = NativeMethods.LoadLibrary("user32.dll");

			_hookProc = HookProc;
			_hookHandle = NativeMethods.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, _hookProc, hInstance, 0);
		}

		~PassiveKeyboardMonitor()
		{
			NativeMethods.UnhookWindowsHookEx(_hookHandle);
		}

		private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
		{
			if (code < 0) return NativeMethods.CallNextHookEx(_hookHandle, code, wParam, ref lParam);

			var key = (VirtualKeyCode)lParam.vkCode;
			var args = new KeyEventArgs(key);

			switch ((KeyboardHookType)wParam)
			{
				case KeyboardHookType.WM_KEYDOWN:
				case KeyboardHookType.WM_SYSKEYDOWN:
					KeyDown?.BeginInvoke(args, null, null);
					break;
				case KeyboardHookType.WM_KEYUP:
				case KeyboardHookType.WM_SYSKEYUP:
					KeyUp?.BeginInvoke(args, null, null);
					break;
			}

			return NativeMethods.CallNextHookEx(_hookHandle, code, wParam, ref lParam);
		}

		#region DLL Imports

		#endregion
	}
}
