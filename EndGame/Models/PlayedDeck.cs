using System;
using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Models
{
	public class PlayedDeck : Deck
	{
		private int _turns;

		[JsonProperty("turns")]
		public int Turns
		{
			get { return _turns; }
			set { Set(() => Turns, ref _turns, value); }
		}

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
			cards.ForEach(c => Cards.Add(new SingleCard(c)));
		}

		// TODO should this be here
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