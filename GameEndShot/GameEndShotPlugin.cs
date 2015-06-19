using System;
using System.Windows.Controls;

using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker;
using MahApps.Metro.Controls.Dialogs;

namespace HDT.Plugins.GameEndShot
{
    public class GameEndShotPlugin : IPlugin
    {
		private MenuItem ShotMenuItem;
		private Controls.PluginSettings SettingsDialog;

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
			if (SettingsDialog != null)
				Helper.MainWindow.ShowMetroDialogAsync(SettingsDialog);
		}

		public void OnLoad()
		{
			ShotMenuItem = new Controls.PluginMenu();
			SettingsDialog = new Controls.PluginSettings();
			GameEvents.OnGameEnd.Add(GameEnd.ScreenShot);
		}

		public void OnUnload()
		{
			SettingsDialog.RequestCloseAsync();
		}

		public void OnUpdate()
		{
			
		}
		
	}
}

