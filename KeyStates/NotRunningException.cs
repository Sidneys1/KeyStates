using System;

namespace KeyStates
{
	public class NotRunningException : Exception
	{
		public NotRunningException(string message) : base(message)
		{
		}
	}
}
