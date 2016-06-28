using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Hearthstone;

namespace HDT.Plugins.EndGame.Services
{
	public class TrackerRepository : ITrackerRepository
	{
		public async Task<List<ArchetypeDeck>> GetAllArchetypeDecks()
		{
			return await Task.Run(() =>
			{
				return DeckList.Instance.Decks
					.Where(d => !d.Archived && d.TagList.ToLower().Contains("archetype"))
					.Select(d => new ArchetypeDeck(d.Name, d.GetClass))
					.ToList();
			}).ConfigureAwait(false);
		}

		public Task GetGameNote()
		{
			throw new NotImplementedException();
		}

		public async Task<PlayedDeck> GetOpponentDeck()
		{
			return await Task.Run(() =>
			{
				var deck = new PlayedDeck();
				if (Core.Game.IsRunning)// && !Core.Game.IsInMenu)
				{
					var game = Core.Game.CurrentGameStats;

					if (game != null && game.CanGetOpponentDeck)
					{
						foreach (var card in game.OpponentCards)
						{
							var c = Database.GetCardFromId(card.Id);
							if (c != null && c != Database.UnknownCard)
							{
								c.Count = card.Count;
								deck.Cards.Add(new Models.Card(c.Id, c.LocalizedName, c.Count, c.Background));
							}
						}
					}
					else
					{
						var live = Core.Game.Opponent.OpponentCardList; // includes created etc.
						deck.Cards = live.Select(x => new Models.Card(x.Id, x.LocalizedName, x.Count, x.Background)).ToList();
					}
				}
				return deck;
			}).ConfigureAwait(false);
		}

		public Task UpdateGameNote(string text)
		{
			throw new NotImplementedException();
		}
	}
}