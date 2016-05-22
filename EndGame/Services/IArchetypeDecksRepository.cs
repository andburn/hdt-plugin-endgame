using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface IArchetypeDecksRepository
	{
		Task<List<ArchetypeDeck>> GetAllDecks();

		Task<ArchetypeDeck> GetDeck(Guid deckId);

		Task<ArchetypeDeck> AddDeck(Deck deck);

		Task<ArchetypeDeck> UpdateDeck(Deck deck);

		Task DeleteDeck(Guid deckId);
	}
}