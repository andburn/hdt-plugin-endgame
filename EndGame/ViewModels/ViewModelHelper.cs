using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
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

		public static IEnumerable<Deck> GetDecksWithArchetypeGames(IDataRepository data)
		{
			var games = data.GetAllGames()
				.Where(g => g.Note.HasArchetype)
				.Select(g => g.Deck.Id)
				.Distinct()
				.ToLookup(x => x);
			var decks = data.GetAllDecks()
				.Where(d => games.Contains(d.Id))
				.ToList();
			return decks;
		}

		public static IEnumerable<ArchetypeRecord> GetArchetypeStats(IDataRepository data, Deck deck)
		{
			var stats = new List<ArchetypeRecord>();
			if (deck != null && deck.Id != null && deck.Id != Guid.Empty)
			{
				var archetypes = new Dictionary<string, ArchetypeRecord>();
				var games = data.GetAllGamesWithDeck(deck.Id);
				foreach (var g in games)
				{
					if (g.Note.HasArchetype)
					{
						var type = g.Note.Archetype;
						if (!archetypes.ContainsKey(type))
						{
							archetypes[type] = new ArchetypeRecord(type, g.OpponentClass);
						}
						if (g.Result == GameResult.WIN)
						{
							archetypes[type].TotalWins++;
						}
						else if (g.Result == GameResult.LOSS)
						{
							archetypes[type].TotalLosses++;
						}
					}					
					else
					{
						if (!archetypes.ContainsKey(ArchetypeRecord.DefaultName))
						{
							archetypes[ArchetypeRecord.DefaultName] = new ArchetypeRecord();
						}
						if (g.Result == GameResult.WIN)
						{
							archetypes[ArchetypeRecord.DefaultName].TotalWins++;
						}
						else if (g.Result == GameResult.LOSS)
						{
							archetypes[ArchetypeRecord.DefaultName].TotalLosses++;
						}
					}
				}
				if (archetypes.Count > 0)
					stats = archetypes.Values.ToList();
				else
					stats.Add(new ArchetypeRecord());
			}
			return stats;
		}
	}
}