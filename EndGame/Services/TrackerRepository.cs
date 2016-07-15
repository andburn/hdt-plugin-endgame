using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HDT.Plugins.EndGame.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;
using HDTCard = Hearthstone_Deck_Tracker.Hearthstone.Card;
using HDTDeck = Hearthstone_Deck_Tracker.Hearthstone.Deck;

namespace HDT.Plugins.EndGame.Services
{
	public class TrackerRepository : ITrackerRepository
	{
		public bool IsInMenu()
		{
			return Core.Game?.IsInMenu ?? false;
		}

		public List<ArchetypeDeck> GetAllArchetypeDecks()
		{
			var decks = DeckList.Instance.Decks
				.Where(d => d.TagList.ToLowerInvariant().Contains("archetype"))
				.ToList();
			var archetypes = new List<ArchetypeDeck>();
			foreach (var d in decks)
			{
				// get the newest version of the deck
				var v = d.VersionsIncludingSelf.OrderByDescending(x => x).FirstOrDefault();
				d.SelectVersion(v);
				if (d == null)
					continue;
				var ad = new ArchetypeDeck(d.Name, KlassKonverter.FromString(d.Class), d.StandardViable);
				ad.Cards = d.Cards
					.Select(x => new Models.Card(x.Id, x.LocalizedName, x.Count, x.Background.Clone()))
					.ToList();
				archetypes.Add(ad);
			}
			return archetypes;
		}

		public Models.Deck GetOpponentDeck()
		{
			var deck = new Models.Deck();
			if (Core.Game.IsRunning)
			{
				var game = Core.Game.CurrentGameStats;
				if (game != null && game.CanGetOpponentDeck)
				{
					Log.Info("oppoent deck available");
					// hero class
					deck.Klass = KlassKonverter.FromString(game.OpponentHero);
					// standard viable, use temp HDT deck
					var hdtDeck = new HDTDeck();
					foreach (var card in game.OpponentCards)
					{
						var c = Database.GetCardFromId(card.Id);
						c.Count = card.Count;
						hdtDeck.Cards.Add(c);
						if (c != null && c != Database.UnknownCard)
						{
							deck.Cards.Add(
								new Models.Card(c.Id, c.LocalizedName, c.Count, c.Background.Clone()));
						}
					}
					deck.IsStandard = hdtDeck.StandardViable;
				}
			}
			return deck;
		}

		public string GetGameNote()
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				return Core.Game.CurrentGameStats.Note;
			}
			return null;
		}

		public void UpdateGameNote(string text)
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				Core.Game.CurrentGameStats.Note = text;
			}
		}

		public void AddDeck(Models.Deck deck)
		{
			HDTDeck d = new HDTDeck();
			var arch = deck as ArchetypeDeck;
			if (arch != null)
				d.Name = arch.Name;
			d.Class = deck.Klass.ToString();
			d.Cards = new ObservableCollection<HDTCard>(deck.Cards.Select(c => Database.GetCardFromId(c.Id)));
			DeckList.Instance.Decks.Add(d);
			// doesn't refresh the deck picker view
		}

		public void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags)
		{
			var deck = Helper.ParseCardString(cards);
			if (deck != null)
			{
				deck.Name = name;
				if (deck.Class != playerClass)
					deck.Class = playerClass;
				if (tags.Any())
				{
					var reloadTags = false;
					foreach (var t in tags)
					{
						if (!DeckList.Instance.AllTags.Contains(t))
						{
							DeckList.Instance.AllTags.Add(t);
							reloadTags = true;
						}
						deck.Tags.Add(t);
					}
					if (reloadTags)
					{
						DeckList.Save();
						Core.MainWindow.ReloadTags();
					}
				}
				// hack time!
				// use MainWindow.ArchiveDeck to update
				// set deck archive to opposite of desired
				deck.Archived = !archive;
				// add and save
				DeckList.Instance.Decks.Add(deck);
				DeckList.Save();
				// now reverse 'archive' of the deck
				// this should refresh all ui elements
				Core.MainWindow.ArchiveDeck(deck, archive);
			}
		}

		public void DeleteAllDecksWithTag(string tag)
		{
			if (string.IsNullOrWhiteSpace(tag))
				return;
			var decks = DeckList.Instance.Decks.Where(d => d.Tags.Contains(tag)).ToList();
			Log.Info($"Deleting {decks.Count} archetype decks");
			foreach (var d in decks)
				DeckList.Instance.Decks.Remove(d);
			if (decks.Any())
				DeckList.Save();
		}

		public string GetGameMode()
		{
			if (Core.Game.IsRunning && Core.Game.CurrentGameStats != null)
			{
				return Core.Game.CurrentGameStats.GameMode.ToString().ToLowerInvariant();
			}
			return string.Empty;
		}
	}
}