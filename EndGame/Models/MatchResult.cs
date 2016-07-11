using System;

namespace HDT.Plugins.EndGame.Models
{
	public class MatchResult : IComparable<MatchResult>
	{
		public const float THRESHOLD = 0.09f;

		public ArchetypeDeck Deck { get; private set; }
		public float Similarity { get; private set; }
		public float Containment { get; private set; }

		public MatchResult(ArchetypeDeck deck, float similarity)
		{
			Deck = deck;
			Similarity = similarity;
		}

		public MatchResult(ArchetypeDeck deck, float similarity, float containment)
		{
			Deck = deck;
			Similarity = similarity;
			Containment = containment;
		}

		public int CompareTo(MatchResult other)
		{
			if (Similarity == other.Similarity)
				return Containment.CompareTo(other.Containment);
			return Similarity.CompareTo(other.Similarity);
		}
	}
}