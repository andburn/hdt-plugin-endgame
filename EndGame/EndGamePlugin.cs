using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Properties;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace HDT.Plugins.EndGame
{
	public class EndGamePlugin : IPlugin
	{
		private MenuItem _endGameMenuItem;
		private static Flyout _settings;

		public static Flyout SettingsFlyout
		{
			get
			{
				if (_settings == null)
					SetSettingsFlyout();
				return _settings;
			}
		}

		public string Author
		{
			get { return "andburn"; }
		}

		public string ButtonText
		{
			get { return "Settings"; }
		}

		public string Description
		{
			get { return "Takes a screenshot of the end game screen."; }
		}

		public MenuItem MenuItem
		{
			get { return _endGameMenuItem; }
		}

		public string Name
		{
			get { return "End Game"; }
		}

		public Version Version
		{
			get { return new Version(0, 3, 2); }
		}

		public void OnButtonPress()
		{
			if (_settings != null)
				_settings.IsOpen = true;
		}

		public void OnLoad()
		{
			CheckForUpdate();
			SaveDefaultNoteSettings();
			ClearDefaultNoteSettings();
			_endGameMenuItem = new Controls.PluginMenu();
			SetSettingsFlyout();
			GameEvents.OnGameEnd.Add(EndGame.ScreenShot);
		}

		public void OnUnload()
		{
			_settings.IsOpen = false;
			RestoreDefaultNoteSettings();
		}

		public void OnUpdate()
		{
		}

		private async void CheckForUpdate()
		{
			var latest = await Github.CheckForUpdate("andburn", "hdt-plugin-endgame", Version);
			if (latest != null)
			{
				await ShowUpdateMessage(latest);
				Log.Info("Update available: " + latest.tag_name, "EndGame");
			}
		}

		private static void SetSettingsFlyout()
		{
			var window = Hearthstone_Deck_Tracker.API.Core.MainWindow;
			var flyouts = window.Flyouts.Items;

			var settings = new Flyout();
			settings.Name = "PluginSettingsFlyout";
			settings.Position = Position.Left;
			Panel.SetZIndex(settings, 100);
			settings.Header = "End Game Settings";
			settings.Content = new Controls.PluginSettings();
			//newflyout.Width = 250;
			//settings.Theme = FlyoutTheme.Accent;
			flyouts.Add(settings);

			_settings = settings;
		}

		public static void SaveDefaultNoteSettings()
		{
			Settings.Default.WasNoteDialogOn = Config.Instance.ShowNoteDialogAfterGame;
			Settings.Default.WasNoteDialogDelayed = Config.Instance.NoteDialogDelayed;
			Settings.Default.WasNoteEnterChecked = Config.Instance.EnterToSaveNote;
			Settings.Default.Save();
		}

		public static void ClearDefaultNoteSettings()
		{
			if (Settings.Default.UseAdvancedShot)
			{
				Config.Instance.ShowNoteDialogAfterGame = false;
				Config.Save();
			}
		}

		public static void RestoreDefaultNoteSettings()
		{
			Config.Instance.ShowNoteDialogAfterGame = Settings.Default.WasNoteDialogOn;
			Config.Instance.NoteDialogDelayed = Settings.Default.WasNoteDialogDelayed;
			Config.Instance.EnterToSaveNote = Settings.Default.WasNoteEnterChecked;
			Config.Save();
		}

		private async Task ShowUpdateMessage(Github.GithubRelease release)
		{
			var settings = new MetroDialogSettings { AffirmativeButtonText = "Get Update", NegativeButtonText = "Close" };

			var result = await Hearthstone_Deck_Tracker.API.Core.MainWindow.ShowMessageAsync("Update Available",
				"For Plugin: \"" + this.Name + "\"", MessageDialogStyle.AffirmativeAndNegative, settings);

			if (result == MessageDialogResult.Affirmative)
				Process.Start(release.html_url);
		}
	}
}