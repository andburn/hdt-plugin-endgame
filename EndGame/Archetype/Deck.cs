using System;
using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Enums;

namespace HDT.Plugins.EndGame.Archetype
{
	public abstract class Deck : ObservableObject
	{
		public Guid Id { get; set; }
		public PlayerClass Klass { get; set; }
		public GameFormat Format { get; set; }
		public List<Card> Cards { get; set; }

		public Deck()
		{
			Id = Guid.NewGuid();
			Cards = new List<Card>();
		}

		public Deck(PlayerClass klass, GameFormat format, List<Card> cards)
			: this()
		{
			Klass = klass;
			Format = format;
			Cards = cards;
		}

		public Deck(string klass, GameFormat format, List<Card> cards)
			: this()
		{
			Klass = Utils.KlassFromString(klass);
			Format = format;
			Cards = cards;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Deck d = obj as Deck;
			if (d == null)
			{
				return false;
			}

			return Id.Equals(d.Id);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public bool Matches(Deck d)
		{
			return Klass == d.Klass && Format == d.Format
				&& Cards.OrderBy(x => x).SequenceEqual(d.Cards.OrderBy(x => x));
		}
	}
}