using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

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
					// also check current player rank is above the threshold setting
					return EndGame.Settings.Get(Strings.RecordRankedArchetypes).Bool
						&& (EndGame.Data.GetPlayerRank() 
							<= EndGame.Settings.Get(Strings.StartRank).Int);

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
                .OrderBy(d => d.Name);
            return decks;
        }

        public static IEnumerable<ArchetypeRecord> GetArchetypeStats(List<Game> games)
        {
            var stats = new List<ArchetypeRecord>();
            var lookup = new Dictionary<string, ArchetypeRecord>();
                
            foreach (var g in games)
            {
                // if opponent class is missing skip game
                if (g.OpponentClass == PlayerClass.ALL)
                {
                    EndGame.Logger.Info($"Skipping game {g.Id}, no opponent class");
                    continue;
                }
                // create an index for the archetype including class
                string index = string.Empty;
                string name = ArchetypeRecord.DefaultName;
                if (g.Note.HasArchetype)
                    name = g.Note.Archetype;                
                else if (!EndGame.Settings.Get(Strings.IncludeUnknown).Bool)
                    // if we don't want to include unknown archetypes skip game
                    continue;
                index = $"{name}.{g.OpponentClass}";
                // update an existing record or add a new one
                if (!lookup.ContainsKey(index))
                    lookup[index] = new ArchetypeRecord(name, g.OpponentClass);
                lookup[index].Update(g.Result);
            }
            if (lookup.Count > 0)
                stats.AddRange(lookup.Values.ToList());

            return stats;
        }
    }
}