using System;
using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.ViewModels
{
	public static class ViewModelHelper
	{
		public static List<MatchResult> MatchArchetypes(Deck deck, List<ArchetypeDeck> archetypes)
		{
			return archetypes
				.Where(a => (a.Klass == deck.Klass || deck.Klass == Klass.Any)
					// archetype must be standard if deck is, else wild or standard ok
					&& (deck.IsStandard && a.IsStandard || !deck.IsStandard))
				.Select(a => new MatchResult(a, deck.Similarity(a)))
				.ToList();
		}
	}

	public class MatchResult : IComparable<MatchResult>
	{
		public ArchetypeDeck Deck { get; private set; }
		public float Similarity { get; private set; }

		public MatchResult(ArchetypeDeck deck, float similarity)
		{
			Deck = deck;
			Similarity = similarity;
		}

		public int CompareTo(MatchResult other)
		{
			return other.Similarity.CompareTo(Similarity);
		}
	}
}