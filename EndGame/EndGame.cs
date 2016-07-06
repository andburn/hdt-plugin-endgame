using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Properties;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using HDT.Plugins.EndGame.ViewModels;
using HDT.Plugins.EndGame.Views;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame
{
	public class EndGame
	{
		private static Flyout _settingsFlyout;
		private static IImageCaptureService _capture;
		private static ITrackerRepository _repository;

		static EndGame()
		{
			_capture = new TrackerCapture();
			_repository = new TrackerRepository();
		}

		public async static void Run()
		{
			// close any already open note windows
			Application.Current.Windows.OfType<NoteView>()
				.ToList()
				.ForEach(x => x.Close());

			ObservableCollection<Screenshot> screenshots = null;
			if (Settings.Default.ScreenshotEnabled && IsEnabledForMode(_repository.GetGameMode()))
			{
				screenshots = await _capture.CaptureSequence(
					Settings.Default.Delay,
					Settings.Default.OutputDir,
					Settings.Default.NumberOfImages,
					Settings.Default.DelayBetweenShots);
			}

			var viewModel = new NoteViewModel(screenshots);
			var view = new NoteView();
			view.DataContext = viewModel;
			view.Show();
		}

		private static bool IsEnabledForMode(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return Settings.Default.RecordRanked;

				case "casual":
					return Settings.Default.RecordCasual;

				case "arena":
					return Settings.Default.RecordArena;

				case "brawl":
					return Settings.Default.RecordBrawl;

				case "friendly":
					return Settings.Default.RecordFriendly;

				case "practice":
					return Settings.Default.RecordPractice;

				case "spectator":
				case "none":
					return Settings.Default.RecordOther;

				default:
					return false;
			}
		}

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