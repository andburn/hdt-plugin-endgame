using System.Collections.Generic;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface ITrackerRepository
	{
		Deck GetOpponentDeck();

		string GetGameNote();

		string GetGameMode();

		void UpdateGameNote(string text);

		List<ArchetypeDeck> GetAllArchetypeDecks();

		void AddDeck(Deck deck);

		void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags);

		void DeleteAllDecksWithTag(string tag);
	}
}