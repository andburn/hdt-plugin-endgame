using GalaSoft.MvvmLight;
using HDT.Plugins.EndGame.Properties;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsArchetypeViewModel : ViewModelBase
	{
		private Settings _settings;

		public Settings Settings
		{
			get { return _settings; }
			set { Set(() => Settings, ref _settings, value); }
		}

		public SettingsArchetypeViewModel()
		{
			Settings = Settings.Default;
		}
	}
}