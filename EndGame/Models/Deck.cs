using System;
using System.Collections.Generic;
using System.Linq;

namespace HDT.Plugins.EndGame.Models
{
	public class Deck
	{
		public string Klass { get; set; }
		public string Format { get; set; }
		public List<Card> Cards { get; set; }

		public Deck()
		{
			Cards = new List<Card>();
		}

		public Deck(string format, string klass)
			: this()
		{
			Klass = klass;
			Format = format;
		}

		public float Similarity(Deck deck)
		{
			float count = (float)deck.Cards.Count;
			if (count <= 0)
				return 0;
			float contained = deck.Cards
				.Where(c => Cards.Contains(c))
				.Count();
			return (float)Math.Round(contained / count, 2);
		}
	}
}