using System;
using System.Xml.Serialization;
using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.EndGame.Archetype
{
	public class Card : IEquatable<Card>, IComparable<Card>
	{
		[XmlAttribute]
		public string Id { get; set; }

		[XmlAttribute]
		public int Count { get; set; }

		public Card()
		{
		}

		public Card(string id, int count)
		{
			Id = id;
			Count = count;
		}

		public Card(TrackedCard card)
		{
			Id = card.Id;
			Count = card.Count;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Card ac = obj as Card;
			if (ac == null)
			{
				return false;
			}

			return Id == ac.Id && Count == ac.Count;
		}

		public virtual bool Equals(Card obj)
		{
			if (obj == null)
			{
				return false;
			}

			return Id == obj.Id && Count == obj.Count;
		}

		public virtual bool Equals(TrackedCard obj)
		{
			if (obj == null)
			{
				return false;
			}

			return Id == obj.Id && Count == obj.Count;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode() ^ Count;
		}

		public int CompareTo(Card other)
		{
			return Equals(other) ? 0 : Id.CompareTo(other.Id); // QU ignore count
		}

		public override string ToString()
		{
			return $"{Id} x{Count}";
		}
	}
}