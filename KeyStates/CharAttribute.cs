using System;

namespace KeyStates
{
	class CharAttribute : Attribute
	{
		public char Char { get; }
		public char ShiftChar { get; }

		public CharAttribute(char ch, char sch = '\0')
		{
			Char = ch;
			ShiftChar = sch == '\0' && char.IsLower(ch.ToString(), 0) ? char.ToUpper(ch) : sch;
		}

	}
}
