using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.Services
{
	public interface IArchetypeImporter
	{
		Task<int> ImportDecks(bool includeWild, bool archive, bool delete, bool removeClass);
	}
}