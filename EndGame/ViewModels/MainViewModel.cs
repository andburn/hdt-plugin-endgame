using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Utilities;
using System.Threading.Tasks;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private static ViewModelBase _settings;

		private static ViewModelBase SettingsVM
		{
			get
			{
				if (_settings == null)
					_settings = new SettingsViewModel();
				return _settings;
			}
		}

		private static NoteViewModelBase _note;

		private static NoteViewModelBase NoteVM
		{
			get
			{
				if (_note == null)
					_note = new NoteViewModel();
				return _note;
			}
		}

		private static NoteViewModelBase _basicNote;

		private static NoteViewModelBase BasicNoteVM
		{
			get
			{
				if (_basicNote == null)
					_basicNote = new BasicNoteViewModel();
				return _basicNote;
			}
		}

		private static NoteViewModelBase _emptyNote;

		private static NoteViewModelBase EmptyNoteVM
		{
			get
			{
				if (_emptyNote == null)
					_emptyNote = new EmptyNoteViewModel();
				return _emptyNote;
			}
		}

		private static ViewModelBase _stats;

		private static ViewModelBase StatsVM
		{
			get
			{
				if (_stats == null)
					_stats = new StatsViewModel();
				return _stats;
			}
		}

		private static NoteViewModelBase _dev;

		private static NoteViewModelBase DevVM
		{
			get
			{
				if (_dev == null)
					_dev = new DevViewModel();
				return _dev;
			}
		}

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
			else if (loc == Strings.NavDev)
			{
				ContentViewModel = DevVM;
				await DevVM.Update();				
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
			var show = true;

			if (EndGame.Settings.Get(Strings.DeveloperMode).Bool)
			{
				EndGame.Logger.Debug($"MainView: DevMode enabled enabling note view");
				viewModel = NoteVM;
			}
			else if (ViewModelHelper.IsDeckAvailable())
			{
				EndGame.Logger.Debug($"MainView: Opponent deck is available");
				if (ViewModelHelper.IsModeEnabledForArchetypes(mode))
				{
					EndGame.Logger.Debug($"MainView: note is enabled for {mode} mode");
					viewModel = NoteVM;
				}
				else if (EndGame.Settings.Get(Strings.ShowRegularNoteBox).Bool)
				{
					EndGame.Logger.Debug($"MainView: enabling regular note box");
					viewModel = BasicNoteVM;
				}
				else
				{
					EndGame.Logger.Debug($"MainView: no note dialog should be displayed");
					// stop the view model being set and updated
					show = false;
				}
			}

			if (show)
			{
				EndGame.Logger.Debug($"MainView: Updating view model");
				ContentViewModel = viewModel;
				await viewModel.Update();
			}
		}
	}
}