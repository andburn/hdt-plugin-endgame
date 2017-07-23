using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.EndGame.Utilities;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private static readonly ViewModelBase SettingsVM = new SettingsViewModel();
		private static readonly NoteViewModelBase NoteVM = new NoteViewModel();
		private static readonly NoteViewModelBase BasicNoteVM = new BasicNoteViewModel();
		private static readonly NoteViewModelBase EmptyNoteVM = new EmptyNoteViewModel();
		private static readonly ViewModelBase StatsVM = new StatsViewModel();

		private string _contentTitle;

		public string ContentTitle
		{
			get { return _contentTitle; }
			set { Set(() => ContentTitle, ref _contentTitle, value); }
		}

		private ViewModelBase _contentViewModel;

		public ViewModelBase ContentViewModel
		{
			get { return _contentViewModel; }
			set { Set(() => ContentViewModel, ref _contentViewModel, value); }
		}

		public RelayCommand<string> NavigateCommand { get; private set; }

		public MainViewModel()
		{
			// set default content to screenshot view
			ContentViewModel = SettingsVM;
			NavigateCommand = new RelayCommand<string>(async x => await OnNavigation(x));
		}

		public async Task OnNavigation(string location)
		{
			var loc = location.ToLower();
			if (loc == Strings.NavSettings)
			{
				ContentViewModel = SettingsVM;
			}
			else if (loc == Strings.NavNote)
			{
				await LoadNote();
			}
			else if (loc == Strings.NavStats)
			{
				ContentViewModel = StatsVM;
				var updatable = ContentViewModel as IUpdatable;
				if (updatable != null)
					await updatable.Update();
			}
			else
			{
				EndGame.Logger.Error($"Unknown Main navigation '{location}'");
				return;
			}

			if (loc.Length > 2)
				ContentTitle = loc.Substring(0, 1).ToUpper() + loc.Substring(1);
		}

		private async Task LoadNote()
		{
			NoteViewModelBase viewModel = EmptyNoteVM;
			var mode = EndGame.Data.GetGameMode();
			if (IsDeckAvailable())
			{
				if (ViewModelHelper.IsModeEnabledForArchetypes(mode))
				{
					viewModel = NoteVM;
				}
				else if (EndGame.Settings.Get(Strings.ShowRegularNoteBox).Bool)
				{
					viewModel = BasicNoteVM;
				}
			}
			else if (EndGame.Settings.Get(Strings.DeveloperMode).Bool)
			{
				viewModel = NoteVM;
			}

			ContentViewModel = viewModel;
			await viewModel.Update();
		}

		public static bool IsDeckAvailable()
		{
			var deck = EndGame.Data.GetOpponentDeck();
			return deck.Cards.Count >= 1 && deck.Class != PlayerClass.ALL;
		}
	}
}