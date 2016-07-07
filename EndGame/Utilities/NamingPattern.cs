using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HDT.Plugins.EndGame.Utilities
{
	public class NamingPattern
	{
		private const string DefaultPattern = "{PlayerName} ({PlayerClass}) VS {OpponentName} ({OpponentClass}) {Date:dd.MM.yyyy_HH.mm}";

		private NamingPattern()
		{
			Pattern = Parse(DefaultPattern);
		}

		public List<string> Pattern { get; private set; }

		private static List<string> Parse(string pattern)
		{
			List<string> list = new List<string>();
			const string regex = @"^([^{}]*)(({[A-Z][^{}]+})([^{}]*))*$";

			if (String.IsNullOrWhiteSpace(pattern))
				pattern = DefaultPattern;

			Regex r = new Regex(regex);

			var match = Regex.Match(pattern, regex);
			if (match.Success)
			{
				var prefix = match.Groups[1].Captures[0].Value;
				if (!String.IsNullOrEmpty(prefix))
					list.Add(prefix);

				var tokens = match.Groups[3].Captures;
				var strings = match.Groups[4].Captures;

				// TODO: check tokens.count == strings.count
				for (int i = 0; i < tokens.Count; i++)
				{
					list.Add(tokens[i].Value);
					if (!String.IsNullOrEmpty(strings[i].Value))
						list.Add(strings[i].Value);
				}
			}
			return list;
		}

		public static bool TryParse(string pattern, out NamingPattern naming)
		{
			naming = new NamingPattern();
			List<string> result = Parse(pattern);
			if (result.Count <= 0)
			{
				return false;
			}
			else
			{
				naming.Pattern = result;
				return true;
			}
		}

		public string Apply(string playerClass, string oppClass, string playerName, string oppName)
		{
			var name = "";
			foreach (var token in Pattern)
			{
				var tokenLower = token.ToLower();
				if (tokenLower == "{playerclass}")
				{
					name += playerClass;
				}
				else if (tokenLower == "{opponentclass}")
				{
					name += oppClass;
				}
				else if (tokenLower == "{playername}")
				{
					name += playerName;
				}
				else if (tokenLower == "{opponentname}")
				{
					name += oppName;
				}
				else if (tokenLower.Contains("{date:"))
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
				if (match.Success)
				{
					var capture = match.Groups["date"].Value;
					var date = DateTime.Now.ToString(capture);
					Console.WriteLine(date);
					if (date == capture)
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