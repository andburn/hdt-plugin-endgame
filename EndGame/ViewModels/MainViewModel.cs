using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
		private static readonly ViewModelBase SettingsVM = new SettingsViewModel();
		private static readonly ViewModelBase NoteVM = new NoteViewModel();

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
			NavigateCommand = new RelayCommand<string>(x => OnNavigation(x));
		}

		public void OnNavigation(string location)
		{
			var loc = location.ToLower();
			ViewModelBase vm = null;
			if (loc == Strings.NavSettings)
			{
				vm = SettingsVM;
			} 
			else if (loc == Strings.NavNote)
			{
				vm = NoteVM;
			}
			else
			{
				vm = SettingsVM;
				EndGame.Logger.Error($"Unknown Main navigation '{location}'");
			}

			// change only if different to current
			if (ContentViewModel != vm)
			{
				ContentViewModel = vm;
				if (loc.Length > 2)
					ContentTitle = loc.Substring(0, 1).ToUpper() + loc.Substring(1);
			}
		}
	}
}