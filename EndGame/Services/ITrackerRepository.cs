using System.Collections.Generic;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface ITrackerRepository
	{
		Deck GetOpponentDeck();

		string GetGameNote();

		void UpdateGameNote(string text);

		List<ArchetypeDeck> GetAllArchetypeDecks();
	}
}