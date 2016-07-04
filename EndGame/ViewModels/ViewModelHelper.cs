using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.ViewModels
{
	public static class ViewModelHelper
	{
		public static List<MatchResult> MatchArchetypes(Deck deck, List<ArchetypeDeck> archetypes)
		{
			return archetypes
				.Where(a => (a.Klass == deck.Klass || deck.Klass == Klass.Any)
					// archetype must be standard if deck is, else either wild or standard ok
					&& (deck.IsStandard && a.IsStandard || !deck.IsStandard))
				.Select(a => new MatchResult(a, deck.Similarity(a), a.Similarity(deck)))
				.OrderByDescending(r => r.Similarity).ThenBy(r => r.Deck.Name)
				.ToList();
		}
	}

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

	public class MatchResult : IComparable<MatchResult>
	{
		public ArchetypeDeck Deck { get; private set; }
		public float Similarity { get; private set; }
		public float SimilarityAlt { get; private set; }

		public MatchResult(ArchetypeDeck deck, float similarity)
		{
			Deck = deck;
			Similarity = similarity;
		}

		public MatchResult(ArchetypeDeck deck, float similarity, float similarityAlt)
		{
			Deck = deck;
			Similarity = similarity;
			SimilarityAlt = similarityAlt;
		}

		public int CompareTo(MatchResult other)
		{
			return Similarity.CompareTo(other.Similarity);
		}
	}
}