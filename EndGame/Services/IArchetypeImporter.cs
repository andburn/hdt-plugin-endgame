using System.Collections.Generic;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface IArchetypeImporter
	{
		Task<List<ArchetypeDeck>> GetArchetypes();
	}
}