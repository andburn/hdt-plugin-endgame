using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		public bool WaitUntilBackInMenu
		{
			get
			{
				return EndGame.Settings.Get(Strings.WaitUntilBackInMenu).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.WaitUntilBackInMenu, value);
				RaisePropertyChanged(Strings.WaitUntilBackInMenu);
			}
		}

		public string IncludeWild
		{
			get
			{
				return EndGame.Settings.Get(Strings.IncludeWild);
			}
			set
			{
				EndGame.Settings.Set(Strings.IncludeWild, value);
				RaisePropertyChanged(Strings.IncludeWild);
			}
		}

		public bool AutoArchiveArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.AutoArchiveArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.AutoArchiveArchetypes, value);
				RaisePropertyChanged(Strings.AutoArchiveArchetypes);
			}
		}

		public bool DeletePreviouslyImported
		{
			get
			{
				return EndGame.Settings.Get(Strings.DeletePreviouslyImported).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.DeletePreviouslyImported, value);
				RaisePropertyChanged(Strings.DeletePreviouslyImported);
			}
		}

		public bool RemoveClassFromName
		{
			get
			{
				return EndGame.Settings.Get(Strings.RemoveClassFromName).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RemoveClassFromName, value);
				RaisePropertyChanged(Strings.RemoveClassFromName);
			}
		}

		public bool ShowRegularNoteBox
		{
			get
			{
				return EndGame.Settings.Get(Strings.ShowRegularNoteBox).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.ShowRegularNoteBox, value);
				RaisePropertyChanged(Strings.ShowRegularNoteBox);
			}
		}

		public bool RecordBrawlArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordBrawlArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordBrawlArchetypes, value);
				RaisePropertyChanged(Strings.RecordBrawlArchetypes);
			}
		}

		public bool RecordCasualArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordCasualArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordCasualArchetypes, value);
				RaisePropertyChanged(Strings.RecordCasualArchetypes);
			}
		}

		public bool RecordFriendlyArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordFriendlyArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordFriendlyArchetypes, value);
				RaisePropertyChanged(Strings.RecordFriendlyArchetypes);
			}
		}

		public bool RecordRankedArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordRankedArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordRankedArchetypes, value);
				RaisePropertyChanged(Strings.RecordRankedArchetypes);
			}
		}

		public bool RecordOtherArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordOtherArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordOtherArchetypes, value);
				RaisePropertyChanged(Strings.RecordOtherArchetypes);
			}
		}

		public bool RecordArenaArchetypes
		{
			get
			{
				return EndGame.Settings.Get(Strings.RecordArenaArchetypes).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.RecordArenaArchetypes, value);
				RaisePropertyChanged(Strings.RecordArenaArchetypes);
			}
		}

		public int StartRank
		{
			get
			{
				return EndGame.Settings.Get(Strings.StartRank).Int;
			}
			set
			{
				EndGame.Settings.Set(Strings.StartRank, value);
				RaisePropertyChanged(Strings.StartRank);
			}
		}

		public bool IsInDevMode
		{
			get
			{
				return EndGame.Settings.Get(Strings.DeveloperMode).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.DeveloperMode, value);
				RaisePropertyChanged("IsInDevMode");
			}
		}

		public bool EnableDebugLog
		{
			get
			{
				return EndGame.Settings.Get(Strings.DebugLog).Bool;
			}
			set
			{
				EndGame.Settings.Set(Strings.DebugLog, value);
				RaisePropertyChanged("EnableDebugLog");
			}
		}

		public SettingsViewModel()
		{
			PropertyChanged += SettingsViewModel_PropertyChanged;
		}

		private void SettingsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "EnableDebugLog")
			{
				EndGame.UpdateLogger();
			}
		}
	}
}