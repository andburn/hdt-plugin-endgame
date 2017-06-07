using System;

namespace HDT.Plugins.EndGame
{
	public class ImportException : Exception
	{
		public ImportException()
		{
		}

		public ImportException(string message)
			: base(message)
		{
		}

		public ImportException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}