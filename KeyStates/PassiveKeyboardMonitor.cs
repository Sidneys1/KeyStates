using System;
using System.Collections.Generic;

namespace KeyStates
{
	/// <summary>
	/// A Passive Keyboard Monitor (Uses Windows Hooks)
	/// </summary>
	public static class PassiveKeyboardMonitor
	{
		#region Private Variables

		private static IntPtr _hookHandle;
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private static readonly KeyboardHookProc HookProc;
		private static readonly List<VirtualKeyCode> DownKeys = new List<VirtualKeyCode>();
		private static readonly IntPtr HInstance;

		#endregion

		#region Events

		public static event KeyEvent KeyDown;
		public static event KeyEvent KeyUp;
		public static event KeyEvent KeyPressed;

		#endregion

		#region Properties

		public static bool IsShiftPressed
			=>
				DownKeys.Contains(VirtualKeyCode.SHIFT) || DownKeys.Contains(VirtualKeyCode.LSHIFT) ||
				DownKeys.Contains(VirtualKeyCode.RSHIFT);

		public static bool IsControlPressed
			=>
				DownKeys.Contains(VirtualKeyCode.CONTROL) || DownKeys.Contains(VirtualKeyCode.LCONTROL) ||
				DownKeys.Contains(VirtualKeyCode.RCONTROL);

		public static bool IsAltPressed
			=>
				DownKeys.Contains(VirtualKeyCode.MENU) || DownKeys.Contains(VirtualKeyCode.LMENU) ||
				DownKeys.Contains(VirtualKeyCode.RMENU);

		public static bool IsAltGrPressed => DownKeys.Contains(VirtualKeyCode.RMENU);

		#endregion

		#region Ctor

		static PassiveKeyboardMonitor()
		{
			HInstance = NativeMethods.LoadLibrary("user32.dll");
			HookProc = DoHookProc;
		}

		#endregion

		#region Private Methods

		private static int DoHookProc(int code, int wParam, ref KeyboardHookStruct lParam)
		{
			if (code < 0) return NativeMethods.CallNextHookEx(_hookHandle, code, wParam, ref lParam);

			var key = (VirtualKeyCode)lParam.vkCode;
			var args = new KeyEventArgs(key);

			switch ((KeyboardHookType)wParam)
			{
				case KeyboardHookType.WM_KEYDOWN:
				case KeyboardHookType.WM_SYSKEYDOWN:
					FireKeyDown(args);
					break;
				case KeyboardHookType.WM_KEYUP:
				case KeyboardHookType.WM_SYSKEYUP:
					FireKeyUp(args);
					break;
			}

			return NativeMethods.CallNextHookEx(_hookHandle, code, wParam, ref lParam);
		}

		#region Fire Events

		private static void FireKeyUp(KeyEventArgs args)
		{
			if (DownKeys.Contains(args.Key))
				DownKeys.Remove(args.Key);

			KeyUp?.Invoke(args);
		}

		private static void FireKeyDown(KeyEventArgs args)
		{
			if (!DownKeys.Contains(args.Key))
				DownKeys.Add(args.Key);

			if (!IsControlPressed && !IsAltPressed && args.Key.IsTextKey())
				FireKeyPressed(args);

			KeyDown?.Invoke(args);
		}

		private static void FireKeyPressed(KeyEventArgs args)
		{
			KeyPressed?.Invoke(args);
		}

		#endregion

		#endregion

		#region Public Methods

		public static void Start()
		{
			_hookHandle = NativeMethods.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, HookProc, HInstance, 0);
		}

		public static void Stop()
		{
			NativeMethods.UnhookWindowsHookEx(_hookHandle);
		}

		public static bool IsKeyPressed(VirtualKeyCode testKey) => DownKeys.Contains(testKey);

		#endregion
	}
}
