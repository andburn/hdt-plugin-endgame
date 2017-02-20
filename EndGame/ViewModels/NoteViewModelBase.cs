using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public abstract class NoteViewModelBase : ViewModelBase, IUpdatable
	{
		public abstract void Update();
	}
}