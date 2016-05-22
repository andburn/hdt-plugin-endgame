using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;
using Hearthstone_Deck_Tracker.Utility.Logging;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Services
{
	public class ArchetypeDecksFileRepository : IArchetypeDecksRepository
	{
		private static readonly string _file = @"E:\Dump\decks.json";

		public Task<ArchetypeDeck> AddDeck(Deck deck)
		{
			throw new NotImplementedException();
		}

		public Task DeleteDeck(Guid deckId)
		{
			throw new NotImplementedException();
		}

		public async Task<List<ArchetypeDeck>> GetAllDecks()
		{
			List<ArchetypeDeck> decks = new List<ArchetypeDeck>();
			try
			{
				using (FileStream fs = new FileStream(_file, FileMode.Open))
				using (StreamReader sr = new StreamReader(fs))
				{
					var text = await sr.ReadToEndAsync().ConfigureAwait(false);
					decks = JsonConvert.DeserializeObject<List<ArchetypeDeck>>(text);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			return decks;
		}

		public async Task<ArchetypeDeck> GetDeck(Guid deckId)
		{
			var decks = await GetAllDecks();
			return decks.FirstOrDefault(x => x.Id == deckId);
		}

		public Task<ArchetypeDeck> UpdateDeck(Deck deck)
		{
			throw new NotImplementedException();
		}
	}
}