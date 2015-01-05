using System;

namespace KeyStates.ConsoleTest
{
	class Program
	{
		private static int _x;
		static void Main()
		{

			KeyboardMonitor.KeyDown += args =>
			{
				Console.SetCursorPosition(0, 4);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Key Down:\t{0}\t\t\t", args.Key);
			};
			KeyboardMonitor.KeyUp += args =>
			{
				Console.SetCursorPosition(0, 3);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Key Up:\t\t{0}\t\t\t", args.Key);
			};
			KeyboardMonitor.KeyPressed += args =>
			{
				Console.SetCursorPosition(0, 2);
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.WriteLine("Key Pressed:\t{0} : {1}\t\t\t", args.Key,
					args.Key.ToChar(KeyboardMonitor.IsKeyPressed(VirtualKeyCode.LSHIFT)));
			};
			KeyboardMonitor.TextKeyPressed += args =>
			{
				Console.SetCursorPosition(0, 1);
				Console.ForegroundColor = ConsoleColor.Gray;
				var arg0 = args.Key.ToChar(KeyboardMonitor.IsKeyPressed(VirtualKeyCode.LSHIFT));
				Console.WriteLine("Text:\t\t{0}\t\t\t", arg0);

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

			KeyboardMonitor.Start();

			Console.ReadLine();
			KeyboardMonitor.Stop();
		}
	}
}
