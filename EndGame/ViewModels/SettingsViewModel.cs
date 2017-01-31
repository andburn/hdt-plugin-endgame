using GalaSoft.MvvmLight;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		private SettingsScreenshotViewModel _screenshotSettings;

		public SettingsScreenshotViewModel ScreenshotSettings
		{
			get { return _screenshotSettings; }
			set { Set(() => ScreenshotSettings, ref _screenshotSettings, value); }
		}

		private SettingsArchetypeViewModel _archetypeSettings;

		public SettingsArchetypeViewModel ArchetypeSettings
		{
			get { return _archetypeSettings; }
			set { Set(() => ArchetypeSettings, ref _archetypeSettings, value); }
		}

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

		public bool ScreenshotEnabled
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "ScreenshotEnabled").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "ScreenshotEnabled", value);
				RaisePropertyChanged("ScreenshotEnabled");
			}
		}

		public SettingsViewModel()
		{
			ScreenshotSettings = new SettingsScreenshotViewModel();
			ArchetypeSettings = new SettingsArchetypeViewModel();
		}
	}
}