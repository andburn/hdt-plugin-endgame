using System;
using System.Collections.Generic;
using System.Linq;

namespace HDT.Plugins.EndGame.Models
{
	public class Deck
	{
		public Klass Klass { get; set; }
		public bool IsStandard { get; set; }
		public List<Card> Cards { get; set; }

		public Deck()
		{
			Cards = new List<Card>();
		}

		public Deck(Klass klass, bool standard)
			: this()
		{
			Klass = klass;
			IsStandard = standard;
		}

		/// <summary>
		/// Uses the Jaccard index to give a indication of similarity
		/// between the two decks. </summary>
		/// <returns>Returns a float between 0 and 1 inclusive </returns>
		public virtual float Similarity(Deck deck)
		{
			if (deck == null)
				return 0;

			var lenA = Cards.Sum(x => x.Count);
			var lenB = deck.Cards.Sum(x => x.Count);
			var lenAnB = 0;

			if (lenA == 0 && lenB == 0)
				return 1;

			foreach (var i in Cards)
			{
				foreach (var j in deck.Cards)
				{
					if (i.Equals(j))
					{
						lenAnB += Math.Min(i.Count, j.Count);
					}
				}
			}

			return (float)Math.Round((float)lenAnB / (lenA + lenB - lenAnB), 2);
		}
	}
}