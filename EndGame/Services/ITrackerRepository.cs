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

		void AddDeck(Deck deck);

		void AddDeck(string name, string playerClass, string cards, params string[] tags);

		void DeleteAllDecksWithTag(string tag);
	}
}