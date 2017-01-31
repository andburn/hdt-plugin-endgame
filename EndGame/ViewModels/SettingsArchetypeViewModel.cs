using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Properties;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsArchetypeViewModel : ViewModelBase
	{
		public bool ArchetypesEnabled
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "ArchetypesEnabled").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "ArchetypesEnabled", value);
				RaisePropertyChanged("ArchetypesEnabled");
			}
		}

		public bool AutoArchiveArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "AutoArchiveArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "AutoArchiveArchetypes", value);
				RaisePropertyChanged("AutoArchiveArchetypes");
			}
		}

		public bool DeletePreviouslyImported
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "DeletePreviouslyImported").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "DeletePreviouslyImported", value);
				RaisePropertyChanged("DeletePreviouslyImported");
			}
		}

		public bool RemoveClassFromName
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RemoveClassFromName").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RemoveClassFromName", value);
				RaisePropertyChanged("RemoveClassFromName");
			}
		}

		public bool CloseNoteWithEnter
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "CloseNoteWithEnter").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "CloseNoteWithEnter", value);
				RaisePropertyChanged("CloseNoteWithEnter");
			}
		}

		public bool RecordBrawlArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordBrawlArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordBrawlArchetypes", value);
				RaisePropertyChanged("RecordBrawlArchetypes");
			}
		}

		public bool RecordCasualArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordCasualArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordCasualArchetypes", value);
				RaisePropertyChanged("RecordCasualArchetypes");
			}
		}

		public bool RecordFriendlyArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordFriendlyArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordFriendlyArchetypes", value);
				RaisePropertyChanged("RecordFriendlyArchetypes");
			}
		}

		public bool RecordRankedArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordRankedArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordRankedArchetypes", value);
				RaisePropertyChanged("RecordRankedArchetypes");
			}
		}

		public bool RecordOtherArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordOtherArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordOtherArchetypes", value);
				RaisePropertyChanged("RecordOtherArchetypes");
			}
		}

		public bool RecordArenaArchetypes
		{
			get
			{
				return EndGame.Settings.Get("Archetypes", "RecordArenaArchetypes").Bool;
			}
			set
			{
				EndGame.Settings.Set("Archetypes", "RecordArenaArchetypes", value);
				RaisePropertyChanged("RecordArenaArchetypes");
			}
		}

		public SettingsArchetypeViewModel()
		{
			
		}
	}
}