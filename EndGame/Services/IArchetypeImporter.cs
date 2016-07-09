using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.Services
{
	public interface IArchetypeImporter
	{
		Task<int> ImportDecks(bool archive, bool delete, bool removeClass);
	}
}