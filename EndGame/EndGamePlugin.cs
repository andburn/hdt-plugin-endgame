using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Utilities;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.EndGame
{
	public class EndGamePlugin : IPlugin
	{
		private MenuItem _menu;

		public string Author
		{
			get
			{
				return "andburn";
			}
		}

		public string ButtonText
		{
			get
			{
				return "Settings";
			}
		}

		public string Description
		{
			get
			{
				return "Adds extra functionality to the built-in end of game note window. Including, victory/defeat screenshots and opponent deck archetypes.";
			}
		}

		public MenuItem MenuItem
		{
			get
			{
				if (_menu == null)
					_menu = new EndGameMenu();
				return _menu;
			}
		}

		public string Name
		{
			get
			{
				return "EndGame";
			}
		}

		public Version Version
		{
			get
			{
				return new Version(0, 4, 2);
			}
		}

		public void OnButtonPress()
		{
			EndGame.ShowSettings();
		}

		public async void OnLoad()
		{
			Config.Instance.ShowNoteDialogAfterGame = false;
			await CheckForUpdate();
			GameEvents.OnGameEnd.Add(EndGame.Run);
		}

		public void OnUnload()
		{
			EndGame.CloseOpenNoteWindows();
			EndGame.CloseSettings();
			EndGame.CloseNotification();
		}

		public void OnUpdate()
		{
		}

		public async Task CheckForUpdate()
		{
			var latest = await Github.CheckForUpdate("andburn", "hdt-plugin-endgame", Version);
			if (latest != null)
			{
				EndGame.Notify("Plugin Update Available", $"[DOWNLOAD]({latest.html_url}) EndGame v{latest.tag_name}", 0,
					"download", () => Process.Start(latest.html_url));
				Log.Info("Update available: v" + latest.tag_name, "EndGame");
			}
		}
	}
}