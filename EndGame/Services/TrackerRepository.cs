using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.EndGame.Services
{
	public class TrackerRepository : ITrackerRepository
	{
		public List<ArchetypeDeck> GetAllArchetypeDecks()
		{
			// TODO allow to be archived?
			var decks = DeckList.Instance.Decks
				.Where(d => !d.Archived && d.TagList.ToLower().Contains("archetype"))
				.ToList();
			var archetypes = new List<ArchetypeDeck>();
			foreach (var d in decks)
			{
				var ad = new ArchetypeDeck(d.Name, d.StandardViable ? "Standard" : "Wild", d.Class);
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
					foreach (var card in game.OpponentCards)
					{
						var c = Database.GetCardFromId(card.Id);
						if (c != null && c != Database.UnknownCard)
						{
							deck.Cards.Add(
								new Models.Card(c.Id, c.LocalizedName, card.Count, c.Background.Clone()));
						}
					}
				}
				else
				{
					// TODO Remove
					deck.Cards = GetLiveDeck();
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

		private List<Models.Card> GetLiveDeck()
		{
			Log.Info("using live deck");
			var live = Core.Game.Opponent.OpponentCardList; // includes created etc.
			return live.Select(x => new Models.Card(x.Id, x.LocalizedName, x.Count, x.Background.Clone())).ToList();
		}
	}
}