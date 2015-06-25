using System;
using System.Windows.Controls;

using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using HDT.Plugins.EndGame.Properties;

namespace HDT.Plugins.EndGame
{
    public class EndGamePlugin : IPlugin
    {
		private MenuItem _shotMenuItem;
		private static Flyout _settings;

		public static Flyout SettingsFlyout { 
			get {
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
			get { return "Take a screenshot at end of a game."; }
		}

		public MenuItem MenuItem
		{
			get { return _shotMenuItem; }
		}

		public string Name
		{
			get { return "End Game"; }
		}

		public Version Version
		{
			get { return new Version(0, 1, 0); }
		}

		public void OnButtonPress()
		{
			if (_settings != null)
				_settings.IsOpen = true;
		}

		public void OnLoad()
		{
			SaveDefaultNoteSettings();
			ClearDefaultNoteSettings();
			_shotMenuItem = new Controls.PluginMenu();
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

		private static void SetSettingsFlyout()
		{
			var window = Hearthstone_Deck_Tracker.Helper.MainWindow;
			var flyouts = window.Flyouts.Items;
			
			var settings = new Flyout();
			settings.Name = "PluginSettingsFlyout";
			settings.Position = Position.Left;
			// TODO: how to set Panel.ZIndex
			//newflyout.Width = 250;
			settings.Header = "End Game Settings";
			settings.Content = new Controls.PluginSettings();
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

	}
}

