using System;

namespace KeyStates
{
	public class KeyEventArgs : EventArgs
	{
		public VirtualKeyCode Key { get; }

		public KeyEventArgs(VirtualKeyCode key)
		{
			Key = key;
		}
	}
}
