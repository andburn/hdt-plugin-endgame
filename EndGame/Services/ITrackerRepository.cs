using System.Collections.Generic;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface ITrackerRepository
	{
		Task<PlayedDeck> GetOpponentDeck();

		Task GetGameNote();

		Task UpdateGameNote(string text);

		Task<List<ArchetypeDeck>> GetAllArchetypeDecks();
	}
}