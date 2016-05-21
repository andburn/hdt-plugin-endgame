using Hearthstone_Deck_Tracker.Stats;

namespace HDT.Plugins.EndGame.Archetype
{
	/// <summary>
	/// Ignores card count when determining equality
	/// </summary>

	public class SingleCard : Card
	{
		public SingleCard() : base()
		{
		}

		public SingleCard(string id) : base(id, 1)
		{
		}

		public SingleCard(string id, int count) : base(id, count)
		{
		}

		public SingleCard(TrackedCard card) : base(card)
		{
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

			return Id == ac.Id;
		}

		public override bool Equals(Card obj)
		{
			if (obj == null)
			{
				return false;
			}

			return Id == obj.Id;
		}

		public override bool Equals(TrackedCard obj)
		{
			if (obj == null)
			{
				return false;
			}

			return Id == obj.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return $"{Id} [x{Count}]";
		}
	}
}