using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame
{
	public class NamingPattern
	{
		private enum Token
		{
			PlayerName,
			PlayerClass,
			OpponentName,
			OpponentClass,
			Date
		}

		private NamingPattern()
		{

		}

		private NamingPattern(string pattern)
		{

		}

		public static bool TryParse(string pattern, out NamingPattern naming)
		{
			const string regex = @"^[^{}]*({[A-Z][^{}]+}[^{}]*)*$";
			naming = null;

			var match = Regex.Match(pattern, regex);
			if(match.Success)
			{
				return true;
			}
			return false;
		}

	}
}
