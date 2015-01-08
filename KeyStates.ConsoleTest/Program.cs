using System;

namespace KeyStates.ConsoleTest
{
	class Program
	{
		private static int _x;
		static void Main()
		{
			const bool active = false;

			if (active)
			{
				ActiveKeyboardMonitor.KeyDown += args =>
				{
					Console.SetCursorPosition(0, 3);
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("{0,9}: {1,-20}", "Key Down", args.Key);
				};
				ActiveKeyboardMonitor.KeyUp += args =>
				{
					Console.SetCursorPosition(0, 2);
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("{0,9}: {1,-20}", "Key Up", args.Key);
				};
				ActiveKeyboardMonitor.KeyPressed += args =>
				{
					Console.SetCursorPosition(0, 1);
					Console.ForegroundColor = ConsoleColor.Gray;

					var arg0 = args.Key.ToChar(ActiveKeyboardMonitor.IsShiftPressed, ActiveKeyboardMonitor.IsKeyPressed(VirtualKeyCode.RMENU));

					Console.WriteLine("{0,9}: {1,-20}", "Character", arg0);

					Console.ForegroundColor = ConsoleColor.White;
					Console.SetCursorPosition(_x++, 0);
					if (_x >= Console.BufferWidth)
						_x = 0;
					if (arg0 == '\b')
					{
						_x = Math.Max(_x - 2, 0);
						Console.SetCursorPosition(_x, 0);
						Console.Write(' ');
					}
					else
						Console.Write(arg0);
				};

				ActiveKeyboardMonitor.Start();

				Console.ReadLine();
				ActiveKeyboardMonitor.Stop();
			}
			else
			{
				PassiveKeyboardMonitor.KeyDown += args => {
					Console.SetCursorPosition(0, 3);
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("{0,9}: {1,-20}", "Key Down", args.Key);
				};
				PassiveKeyboardMonitor.KeyUp += args => {
					Console.SetCursorPosition(0, 2);
					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.WriteLine("{0,9}: {1,-20}", "Key Up", args.Key);
				};

				PassiveKeyboardMonitor.Start();

				Console.ReadLine();
				PassiveKeyboardMonitor.Stop();
			}
		}
	}
}
