using System;
using System.Collections.Generic;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Tests
{
	internal static class TestHelper
	{
		internal static List<Card> CreateCards(string str)
		{
			List<Card> list = new List<Card>();
			var cards = str.Split(';');
			foreach (var c in cards)
			{
				if (string.IsNullOrWhiteSpace(c))
					continue;
				var pair = c.Split(':');
				var card = new Card();
				card.Id = pair[0];
				card.Count = int.Parse(pair[1]);
				list.Add(card);
			}
			if (list.Count <= 0)
				throw new Exception("Failed to create deck from ids");
			return list;
		}

		internal static Deck CreateDeck(Klass klass, bool standard, string str)
		{
			var deck = new Deck();
			deck.Klass = klass;
			deck.IsStandard = standard;
			deck.Cards = CreateCards(str);
			return deck;
		}

		internal static Deck CreateDeck(Klass klass, string str)
		{
			return CreateDeck(klass, true, str);
		}

		internal static Deck CreateDeck(string str)
		{
			return CreateDeck(Klass.Any, true, str);
		}
	}
}