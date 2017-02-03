using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.Common.Controls.SlidePanels;
using HDT.Plugins.Common.Plugin;
using HDT.Plugins.Common.Providers;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Settings;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using HDT.Plugins.EndGame.ViewModels;
using HDT.Plugins.EndGame.Views;
using MahApps.Metro.Controls;

namespace HDT.Plugins.EndGame
{
	[Name("End Game")]
	[Description("Adds extra functionality to the built-in end of game note window. Including, victory/defeat screenshots and opponent deck archetypes.")]
	public class EndGame : PluginBase
	{
		public static readonly IUpdateService Updater;
		public static readonly ILoggingService Logger;
		public static readonly IDataRepository Data;
		public static readonly IEventsService Events;
		public static readonly IGameClientService Client;
		public static readonly IConfigurationRepository Config;
		public static readonly Settings Settings;

		private MenuItem _menuItem;

		private static Flyout _settingsFlyout;
		private static Flyout _notificationFlyout;
		private static IImageCaptureService _capture;

		static EndGame()
		{
			// initialize services
			var resolver = Injector.Instance.Container;
			Updater = resolver.GetInstance<IUpdateService>();
			Logger = resolver.GetInstance<ILoggingService>();
			Data = resolver.GetInstance<IDataRepository>();
			Events = resolver.GetInstance<IEventsService>();
			Client = resolver.GetInstance<IGameClientService>();
			Config = resolver.GetInstance<IConfigurationRepository>();
			// load settings
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "HDT.Plugins.EndGame.Resources.Default.ini";
			Settings = new Settings(assembly.GetManifestResourceStream(resourceName), "EndGame");
			// other
			_capture = new TrackerCapture();
			_notificationFlyout = CreateDialogFlyout();
			_settingsFlyout = CreateSettingsFlyout();
		}

		public override MenuItem MenuItem
		{
			get
			{
				if (_menuItem == null)
					_menuItem = CreatePluginMenu();
				return _menuItem;
			}
		}

		private MenuItem CreatePluginMenu()
		{
			var pm = new PluginMenu("End Game", "trophy");
			pm.Append("Import Meta Decks", 
				new RelayCommand(async () => await ImportMetaDecks()));
			pm.Append("Settings", 
				new RelayCommand(() => ShowSettings()));
			return pm.Menu;
		}

		public override void OnButtonPress()
		{
			EndGame.ShowSettings();
		}

		public override async void OnLoad()
		{
			try
			{
				Config.Set("ShowNoteDialogAfterGame", false);
			}
			catch(Exception e)
			{
				Logger.Error(e);
			}
			await UpdateCheck("EndGame", "hdt-plugin-endgame");
			Events.OnGameEnd(EndGame.Run);
		}

		public override void OnUnload()
		{
			EndGame.CloseOpenNoteWindows();
			EndGame.CloseSettings();
		}

		public async static void Run()
		{
			try
			{
				var mode = Data.GetGameMode();

				// close any already open note windows
				CloseOpenNoteWindows();

				// take the screenshots
				var screenshots = await Capture(mode);
				// check what features are enabled
				if (Settings.Get("Archetypes", "ArchetypesEnabled").Bool && IsModeEnabledForArchetypes(mode))
				{
					var viewModel = new NoteViewModel(screenshots);
					var view = new NoteView();
					view.DataContext = viewModel;
					await WaitUntilInMenu();
					view.Show();
				}
				else if (Settings.Get("ScreenShot", "ScreenshotEnabled").Bool && IsModeEnabledForScreenshots(mode))
				{
					var viewModel = new BasicNoteViewModel(screenshots);
					var view = new BasicNoteView();
					view.DataContext = viewModel;
					await WaitUntilInMenu();
					view.Show();
				}
				// else both disabled, do nothing
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("EndGame Error", e.Message, 15, "error", null);
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

		public static void Notify(string title, string message, int autoClose, string icon = null, Action action = null)
		{
			SlidePanelManager
				.Notification(title, message, icon, action)
				.AutoClose(autoClose);
		}

		public static async Task ImportMetaDecks()
		{
			try
			{
				IArchetypeImporter importer = new SnapshotImporter(new HttpClient(), Data, Logger);
				var count = await importer.ImportDecks(
					Settings.Get("Archetypes", "AutoArchiveArchetypes").Bool,
					Settings.Get("Archetypes", "DeletePreviouslyImported").Bool,
					Settings.Get("Archetypes", "RemoveClassFromName").Bool);
				Notify("Import Complete", $"{count} decks imported", 10);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("Import Failed", e.Message, 15, "error", null);
			}
		}

		private static async Task<ObservableCollection<Screenshot>> Capture(string mode)
		{
			ObservableCollection<Screenshot> screenshots = null;
			try
			{
				if (Settings.Get("ScreenShot", "ScreenshotEnabled").Bool && IsModeEnabledForScreenshots(mode))
				{
					screenshots = await _capture.CaptureSequence(
						Settings.Get("ScreenShot", "Delay").Int,
						Settings.Get("ScreenShot", "OutputDir"),
						Settings.Get("ScreenShot", "NumberOfImages").Int,
						Settings.Get("ScreenShot", "DelayBetweenShots").Int);
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
				Notify("Screen Capture Failed", e.Message, 15, "error", null);
			}
			return screenshots;
		}

		private static bool IsModeEnabledForArchetypes(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return Settings.Get("Archetypes", "RecordRankedArchetypes").Bool;

				case "casual":
					return Settings.Get("Archetypes", "RecordCasualArchetypes").Bool;

				case "brawl":
					return Settings.Get("Archetypes", "RecordBrawlArchetypes").Bool;

				case "friendly":
					return Settings.Get("Archetypes", "RecordFriendlyArchetypes").Bool;

				case "arena":
					return Settings.Get("Archetypes", "RecordArenaArchetypes").Bool;

				default:
					return Settings.Get("Archetypes", "RecordOtherArchetypes").Bool;
			}
		}

		private static bool IsModeEnabledForScreenshots(string mode)
		{
			switch (mode.ToLowerInvariant())
			{
				case "ranked":
					return Settings.Get("ScreenShot", "RecordRanked").Bool;

				case "casual":
					return Settings.Get("ScreenShot", "RecordCasual").Bool;

				case "arena":
					return Settings.Get("ScreenShot", "RecordArena").Bool;

				case "brawl":
					return Settings.Get("ScreenShot", "RecordBrawl").Bool;

				case "friendly":
					return Settings.Get("ScreenShot", "RecordFriendly").Bool;

				case "practice":
					return Settings.Get("ScreenShot", "RecordPractice").Bool;

				case "spectator":
				case "none":
					return Settings.Get("ScreenShot", "RecordOther").Bool;

				default:
					return false;
			}
		}

		private static async Task WaitUntilInMenu()
		{
			var timeout = 30000;
			var wait = 1000;
			var elapsed = 0;
			while (!Client.IsInMenu())
			{
				await Task.Delay(wait);
				elapsed += wait;
				if (elapsed >= timeout)
					return;
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
			var metroWindow = Client.MainWindow() as MetroWindow;
			metroWindow.Flyouts.Items.Add(settings);
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
			var metroWindow = Client.MainWindow() as MetroWindow;
			metroWindow.Flyouts.Items.Add(dialog);
			return dialog;
		}

		private async Task UpdateCheck(string name, string repo)
		{
			var uri = new Uri($"https://api.github.com/repos/andburn/{repo}/releases");
			Logger.Debug("update uri = " + uri);
			try
			{
				var latest = await Updater.CheckForUpdate(uri, Version);
				if (latest.HasUpdate)
				{
					Logger.Info($"Plugin Update available ({latest.Version})");
					SlidePanelManager
						.Notification("Plugin Update Available",
							$"[DOWNLOAD]({latest.DownloadUrl}) {name} v{latest.Version}",
							"download3",
							() => Process.Start(latest.DownloadUrl))
						.AutoClose(10);
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}
	}
}