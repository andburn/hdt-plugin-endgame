using System;
using System.Collections.Generic;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Enums;
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

		internal static Deck CreateDeck(PlayerClass klass, bool standard, string str)
		{
			var deck = new Deck();
			deck.Class = klass;
			deck.IsStandard = standard;
			deck.Cards = CreateCards(str);
			return deck;
		}

		internal static ArchetypeDeck CreateDeck(string name, PlayerClass klass, bool standard, string str)
		{
			var deck = new ArchetypeDeck();
			deck.Name = name;
			deck.Class = klass;
			deck.IsStandard = standard;
			deck.Cards = CreateCards(str);
			return deck;
		}
	}
}