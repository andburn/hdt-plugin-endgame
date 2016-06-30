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

		public float Similarity(Deck deck)
		{
			if (deck == null)
				return 0;

			var lenA = Cards.Sum(x => x.Count);
			var lenB = deck.Cards.Sum(x => x.Count);
			var lenAnB = 0; // intersection
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

			if (lenA == 0 && lenB == 0)
				return 1;

			return (float)Math.Round((float)lenAnB / (lenA + lenB - lenAnB), 2);
		}
	}
}