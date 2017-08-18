using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.EndGame.Models;
using System;
using System.Collections.Generic;

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
			deck.Id = Guid.NewGuid();
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

		internal static Game CreateGame(GameResult result, PlayerClass player, PlayerClass opponent, Deck deck, string archetype)
		{
			var game = new Game();
			game.Deck = deck;
			game.Result = result;
			game.PlayerClass = player;
			game.OpponentClass = opponent;
			game.Note = new Note() { Archetype = archetype };
			return game;
		}
	}
}