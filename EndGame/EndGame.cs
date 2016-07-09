using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
		private static Flyout _notificationFlyout;
		private static IImageCaptureService _capture;
		private static ITrackerRepository _repository;

		static EndGame()
		{
			_capture = new TrackerCapture();
			_repository = new TrackerRepository();
			_notificationFlyout = CreateDialogFlyout();
			_settingsFlyout = CreateSettingsFlyout();
		}

		public async static void Run()
		{
			try
			{
				var mode = _repository.GetGameMode();

				// close any already open note windows
				CloseOpenNoteWindows();

				// take the screenshots
				var screenshots = await Capture(mode);
				// check what features are enabled
				if (Settings.Default.ArchetypesEnabled && IsModeEnabledForArchetypes(mode))
				{
					var viewModel = new NoteViewModel(screenshots);
					var view = new NoteView();
					view.DataContext = viewModel;
					view.Show();
				}
				else if (Settings.Default.ScreenshotEnabled)
				{
					var viewModel = new BasicNoteViewModel(screenshots);
					var view = new BasicNoteView();
					view.DataContext = viewModel;
					view.Show();
				}
				// else both disabled, do nothing
			}
			catch (Exception e)
			{
				Log.Error(e);
				Notify("EndGame Error", e.Message, 15);
			}
		}

		public static void CloseOpenNoteWindows()
		{
			foreach (var x in Application.Current.Windows.OfType<NoteView>())
				x.Close();
			foreach (var x in Application.Current.Windows.OfType<BasicNoteView>())
				x.Close();
		}

		public static void ShowSettings()
		{
			if (_settingsFlyout == null)
				_settingsFlyout = CreateSettingsFlyout();
			_settingsFlyout.IsOpen = true;
		}

		public static void CloseSettings()
		{
			if (_settingsFlyout != null)
				_settingsFlyout.IsOpen = false;
		}

		public static void CloseNotification()
		{
			if (_notificationFlyout != null)
				_notificationFlyout.IsOpen = false;
		}

		public static void Notify(string title, string message, int autoClose)
		{
			if (_notificationFlyout == null)
				_notificationFlyout = CreateDialogFlyout();
			_notificationFlyout.Content = new DialogView(_notificationFlyout, title, message, autoClose);
			_notificationFlyout.IsOpen = true;
		}

		public static async Task ImportMetaDecks()
		{
			try
			{
				IArchetypeImporter importer =
					new SnapshotImporter(new HttpClient(), new TrackerRepository());
				var count = await importer.ImportDecks(
					Settings.Default.AutoArchiveArchetypes,
					Settings.Default.DeletePreviouslyImported,
					Settings.Default.RemoveClassFromName);
				Notify("Import Complete", $"{count} decks imported", 10);
			}
			catch (Exception e)
			{
				Log.Error(e);
				Notify("Import Failed", e.Message, 15);
			}
		}

		private static async Task<ObservableCollection<Screenshot>> Capture(string mode)
		{
			ObservableCollection<Screenshot> screenshots = null;
			try
			{
				if (Settings.Default.ScreenshotEnabled && IsModeEnabledForScreenshots(mode))
				{
					screenshots = await _capture.CaptureSequence(
						Settings.Default.Delay,
						Settings.Default.OutputDir,
						Settings.Default.NumberOfImages,
						Settings.Default.DelayBetweenShots);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
				Notify("Screen Capture Failed", e.Message, 15);
			}
			return screenshots;
		}

		private static bool IsModeEnabledForArchetypes(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return Settings.Default.RecordRankedArchetypes;

				case "casual":
					return Settings.Default.RecordCasualArchetypes;

				case "brawl":
					return Settings.Default.RecordBrawlArchetypes;

				case "friendly":
					return Settings.Default.RecordFriendlyArchetypes;

				default:
					return Settings.Default.RecordOtherArchetypes;
			}
		}

		private static bool IsModeEnabledForScreenshots(string mode)
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

		private static Flyout CreateSettingsFlyout()
		{
			var settings = new Flyout();
			settings.Name = "EndGameSettingsFlyout";
			settings.Position = Position.Left;
			Panel.SetZIndex(settings, 100);
			settings.Header = "End Game Settings";
			settings.Content = new SettingsView();
			Core.MainWindow.Flyouts.Items.Add(settings);
			return settings;
		}

		private static Flyout CreateDialogFlyout()
		{
			var dialog = new Flyout();
			dialog.Name = "EndGameDialogFlyout";
			dialog.Theme = FlyoutTheme.Accent;
			dialog.Position = Position.Bottom;
			dialog.TitleVisibility = Visibility.Collapsed;
			dialog.CloseButtonVisibility = Visibility.Collapsed;
			dialog.IsPinned = false;
			dialog.Height = 50;
			Panel.SetZIndex(dialog, 1000);
			Core.MainWindow.Flyouts.Items.Add(dialog);
			return dialog;
		}
	}
}