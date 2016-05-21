using System;
using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.EndGame.Archetype
{
	public class PlayedDeck : Deck
	{
		public int Turns { get; set; }

		public PlayedDeck()
			: base()
		{
		}

		public PlayedDeck(string klass, Format? format, int turns, List<TrackedCard> cards)
			: base()
		{
			Klass = Utils.KlassFromString(klass);
			Format = Utils.ConvertFormat(format);
			Turns = turns;
			Cards = cards.Select(x => new SingleCard(x)).ToList<Card>();
		}

		public double Similarity(Deck deck)
		{
			double similarity = 0.0;
			if (Klass == deck.Klass && (Format == deck.Format || deck.Format == GameFormat.ANY))
			{
				var found = deck.Cards.Count(c => this.Cards.Contains(c));
				similarity = Math.Round(found / (double)deck.Cards.Count, 2);
			}
			return similarity;
		}
	}
}