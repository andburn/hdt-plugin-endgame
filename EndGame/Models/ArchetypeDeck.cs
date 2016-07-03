using System;
using System.Linq;

namespace HDT.Plugins.EndGame.Models
{
	public class ArchetypeDeck : Deck
	{
		public string Name { get; set; }

		public ArchetypeDeck()
			: base()
		{
		}

		public ArchetypeDeck(string name, Klass klass, bool standard)
			: base(klass, standard)
		{
			Name = name;
		}

		/// <summary>
		/// Determines the percentage of the deck that is contained within
		/// this archetype deck.
		/// </summary>
		/// <returns>Returns a float between 0 and 1 inclusive </returns>
		public override float Similarity(Deck deck)
		{
			if (deck == null)
				return 0;

			var sublen = deck.Cards.Sum(x => x.Count);
			var found = 0;

			if (sublen == 0)
				if (Cards.Count == 0)
					return 1;
				else
					return 0;

			foreach (var i in deck.Cards)
			{
				foreach (var j in Cards)
				{
					if (i.Equals(j))
					{
						found += Math.Min(i.Count, j.Count);
					}
				}
			}

			return (float)Math.Round((float)found / sublen, 2);
		}
	}
}