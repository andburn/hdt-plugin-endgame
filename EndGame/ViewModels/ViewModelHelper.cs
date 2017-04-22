using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public static class ViewModelHelper
	{
		public static void FocusTextBox(TextBox box)
		{
			box.Focus();
			if (!string.IsNullOrEmpty(box.Text) && box.CaretIndex <= 0)
			{
				box.CaretIndex = box.Text.Length;
			}
		}

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

		public static bool IsModeEnabledForArchetypes(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return EndGame.Settings.Get(Strings.RecordRankedArchetypes).Bool;

				case "casual":
					return EndGame.Settings.Get(Strings.RecordCasualArchetypes).Bool;

				case "brawl":
					return EndGame.Settings.Get(Strings.RecordBrawlArchetypes).Bool;

				case "friendly":
					return EndGame.Settings.Get(Strings.RecordFriendlyArchetypes).Bool;

				case "arena":
					return EndGame.Settings.Get(Strings.RecordArenaArchetypes).Bool;

				default:
					return EndGame.Settings.Get(Strings.RecordOtherArchetypes).Bool;
			}
		}
	}
}