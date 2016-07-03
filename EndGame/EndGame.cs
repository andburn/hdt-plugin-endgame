using System.Windows.Controls;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using HDT.Plugins.EndGame.Views;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame
{
	public class EndGame
	{
		private static Flyout _settingsFlyout;

		public static void ShowSettings()
		{
			Log.Debug("Show Settings");
			if (_settingsFlyout == null)
				_settingsFlyout = CreateSettingsFlyout();
			_settingsFlyout.IsOpen = true;
		}

		public static void CloseSettings()
		{
			Log.Debug("Close Settings");
			if (_settingsFlyout != null)
				_settingsFlyout.IsOpen = false;
		}

		public static void ImportMetaDecks()
		{
			Log.Debug("Importing Meta Decks");
			IArchetypeImporter importer =
				new SnapshotImporter(new HttpClient(), new TrackerRepository());
			importer.ImportDecks();
		}

		private static Flyout CreateSettingsFlyout()
		{
			var settings = new Flyout();
			settings.Name = "EndGameSettingsFlyout";
			settings.Position = Position.Left;
			Panel.SetZIndex(settings, 100);
			settings.Header = "End Game Settings";
			settings.Content = new SettingsView();
			//newflyout.Width = 250;
			//settings.Theme = FlyoutTheme.Accent;
			Core.MainWindow.Flyouts.Items.Add(settings);
			return settings;
		}
	}
}