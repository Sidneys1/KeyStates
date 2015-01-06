using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Timers;

namespace KeyStates
{
	public static class KeyboardMonitor
	{
		#region Vars and Properties

		private static readonly byte[] Keys = new byte[256];
		private static readonly List<VirtualKeyCode> DownKeys = new List<VirtualKeyCode>();

		private static readonly Timer Timer = new Timer(10);

		public static double Interval
		{
			get { return Timer.Interval; }
			set { Timer.Interval = value; }
		}

		public static bool IsShiftPressed
		{
			get
			{
				if (Timer.Enabled)
					return DownKeys.Contains(VirtualKeyCode.SHIFT) || DownKeys.Contains(VirtualKeyCode.LSHIFT) || DownKeys.Contains(VirtualKeyCode.RSHIFT);
				throw new NotRunningException("KeyboardMonitor is not currently running.");
			}
		}

		public static bool IsControlPressed
		{
			get
			{
				if (Timer.Enabled)
					return DownKeys.Contains(VirtualKeyCode.CONTROL) || DownKeys.Contains(VirtualKeyCode.LCONTROL) || DownKeys.Contains(VirtualKeyCode.RCONTROL);
				throw new NotRunningException("KeyboardMonitor is not currently running.");
			}
		}

		public static bool IsAltPressed
		{
			get
			{
				if (Timer.Enabled)
					return DownKeys.Contains(VirtualKeyCode.MENU) || DownKeys.Contains(VirtualKeyCode.LMENU)|| DownKeys.Contains(VirtualKeyCode.RMENU);
				throw new NotRunningException("KeyboardMonitor is not currently running.");
			}
		}

		#endregion

		#region Events

		public static KeyEvent KeyDown;
		public static KeyEvent KeyUp;
		public static KeyEvent KeyPressed;

		#endregion

		static KeyboardMonitor()
		{
			Timer.Elapsed += Elapsed;
		}

		private static void Elapsed(object sender, ElapsedEventArgs e)
		{
			//Get state of key to insure the OS vkey array is updated:
			NativeMethods.GetKeyState(VirtualKeyCode.LWIN);

			if (!NativeMethods.GetKeyboardState(Keys))
			{
				var err = Marshal.GetLastWin32Error();
				throw new Win32Exception(err);
			}

			ProcessKey(VirtualKeyCode.SHIFT);
			ProcessKey(VirtualKeyCode.LSHIFT);
			ProcessKey(VirtualKeyCode.RSHIFT);
			ProcessKey(VirtualKeyCode.CONTROL);
			ProcessKey(VirtualKeyCode.MENU);


			for (var index = 0; index < Keys.Length; index++)
			{
				var key = (VirtualKeyCode)index;
				if (key == VirtualKeyCode.CONTROL ||
					key == VirtualKeyCode.SHIFT ||
					key == VirtualKeyCode.LSHIFT ||
					key == VirtualKeyCode.RSHIFT ||
					key == VirtualKeyCode.MENU)
					continue;
				ProcessKey(key);
			}
		}

		private static void ProcessKey(VirtualKeyCode key)
		{
			var state = Keys[(byte)key] & 0x80;
			if (state != 0 && !DownKeys.Contains(key))
			{
				DownKeys.Add(key);
				FireKeyDown(key);
			}
			else if (state == 0 && DownKeys.Contains(key))
			{
				DownKeys.Remove(key);
				FireKeyUp(key);
			}
		}

		private static void FireKeyUp(VirtualKeyCode key)
		{
			KeyUp?.Invoke(new KeyEventArgs(key));
		}

		private static void FireKeyPressed(VirtualKeyCode key)
		{
			KeyPressed?.Invoke(new KeyEventArgs(key));
		}

		private static void FireKeyDown(VirtualKeyCode key)
		{
			KeyDown?.Invoke(new KeyEventArgs(key));

			if (!IsControlPressed && !IsAltPressed && key.ToChar() != '\0')
				FireKeyPressed(key);
		}

		#region Methods

		public static void Start()
		{
			Timer.Start();
		}

		public static void Stop()
		{
			Timer.Stop();
		}

		public static bool IsKeyPressedAsync(VirtualKeyCode testKey)
		{
			var result = NativeMethods.GetKeyState(testKey);

			switch (result)
			{
				// Not pressed or toggled on
				case 0:
					return false;

				// Not pressed, but toggled on
				case 1:
					return false;

				// Pressed and possibly toggled on
				default:
					return true;
			}
		}

		public static bool IsKeyPressed(VirtualKeyCode testKey) => DownKeys.Contains(testKey);

		#endregion
	}
}
