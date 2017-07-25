using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.Services
{
	public interface IArchetypeImporter
	{
        Task<bool> HasUpdate(string type, string previous);

		Task<int> ImportDecks(bool includeWild, bool archive, bool delete, bool removeClass);
	}
}