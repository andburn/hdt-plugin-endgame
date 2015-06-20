using System;
using System.Windows.Controls;

using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;

namespace HDT.Plugins.GameEndShot
{
    public class GameEndShotPlugin : IPlugin
    {
		private MenuItem ShotMenuItem;
		private Flyout _settings;

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
			get { return "Take a screenshot at end of a game"; }
		}

		public MenuItem MenuItem
		{
			get { return ShotMenuItem; }
		}

		public string Name
		{
			get { return "GameEndShot"; }
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
			ShotMenuItem = new Controls.PluginMenu();
			SetSettingsFlyout();
			GameEvents.OnGameEnd.Add(GameEnd.ScreenShot);
		}

		public void OnUnload()
		{
			_settings.IsOpen = false;
		}

		public void OnUpdate()
		{
			
		}

		private void SetSettingsFlyout()
		{
			var window = Hearthstone_Deck_Tracker.Helper.MainWindow;
			var flyouts = window.Flyouts;
			var items = flyouts.Items;
			
			var newflyout = new Flyout();
			newflyout.Name = "PluginSettingsFlyout";
			newflyout.Position = Position.Left;
			// TODO: how to set Panel.ZIndex
			//newflyout.Width = 250;
			newflyout.Header = "Game Shot Settings";
			newflyout.Content = new Controls.PluginSettings();
			newflyout.Theme = FlyoutTheme.Accent;
			items.Add(newflyout);

			_settings = newflyout;	
		}
	}
}

