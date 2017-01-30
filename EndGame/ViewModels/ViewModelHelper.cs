using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Util;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.ViewModels
{
	public static class ViewModelHelper
	{
		public static List<MatchResult> MatchArchetypes(Deck deck, List<ArchetypeDeck> archetypes)
		{
			return archetypes
				.Where(a => (a.Class == deck.Class || deck.Class == PlayerClass.ALL)
					// archetype must be standard if deck is, else either wild or standard ok
					&& (deck.IsStandard && a.IsStandard || !deck.IsStandard))
				.Select(a => new MatchResult(a, deck.Similarity(a), a.Similarity(deck)))
				.OrderByDescending(r => r.Similarity).ThenBy(r => r.Deck.Name)
				.ToList();
		}
	}
}