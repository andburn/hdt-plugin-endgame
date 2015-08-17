using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.EndGame
{
	public class NamingPattern
	{
		private NamingPattern()
		{
			Pattern = new List<string>();
		}

		public List<string> Pattern { get; private set; }

		public static bool TryParse(string pattern, out NamingPattern naming)
		{
			const string regex = @"^([^{}]*)(({[A-Z][^{}]+})([^{}]*))*$";
			naming = new NamingPattern();

			Regex r = new Regex(regex);

			var match = Regex.Match(pattern, regex);
			if(match.Success)
			{
				var prefix = match.Groups[1].Captures[0].Value;
				if (!String.IsNullOrEmpty(prefix))
					naming.Pattern.Add(prefix);

				var tokens = match.Groups[3].Captures;
				var strings = match.Groups[4].Captures;

				// TODO: check tokens.count == strings.count
				for(int i = 0; i < tokens.Count; i++)
				{
					naming.Pattern.Add(tokens[i].Value);
					if(!String.IsNullOrEmpty(strings[i].Value))
						naming.Pattern.Add(strings[i].Value);
				}

				return true;
			}
			return false;
		}

		public string Apply(GameStats game)
		{
			var name = "";
			foreach(var token in Pattern)
			{
				var tokenLower = token.ToLower();
				if(tokenLower == "{playerclass}")
				{
					name += game.PlayerHero;
				}
				else if(tokenLower == "{opponentclass}")
				{
					name += game.OpponentHero;
				}
				else if(tokenLower == "{playername}")
				{
					name += game.PlayerName;
				}
				else if(tokenLower == "{opponentname}")
				{
					name += game.OpponentName;
				}
				else if(tokenLower.Contains("{date:"))
				{
					name += ParseDate(token);
				}
				else
				{
					name += token;
				}
			}
			return name;
		}

		private string ParseDate(string format)
		{
			var defaultDate = DateTime.Now.ToString("dd.MM.yyyy_HH.mm");
			try
			{
				const string regex = "{[Dd]ate:(?<date>(.*?))}";
				var match = Regex.Match(format, regex);
				if(match.Success)
				{
					var capture = match.Groups["date"].Value;
					var date = DateTime.Now.ToString(capture);
					Console.WriteLine(date);
					if(date == capture)
						return defaultDate;
					else
						return date;
				}
				return defaultDate;
			}
			catch
			{
				// error parsing date, use default format
				return defaultDate;
			}
		}

	}
}
