using System;
using GalaSoft.MvvmLight;
using Hearthstone_Deck_Tracker.Stats;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Models
{
	public class Card : ObservableObject, IEquatable<Card>, IComparable<Card>
	{
		[JsonProperty("id")]
		public string Id { get; private set; }

		[JsonProperty("count")]
		public int Count { get; private set; }

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

		// TODO: comparing by card id, useful?
		public int CompareTo(Card other)
		{
			return Equals(other) ? 0 : Id.CompareTo(other.Id);
		}

		public override string ToString()
		{
			return $"{Id} x{Count}";
		}
	}
}