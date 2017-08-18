using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Utilities;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.ViewModels
{
	public abstract class NoteViewModelBase : ViewModelBase, IUpdatable
	{
		public virtual async Task Update()
		{
		}
	}
}