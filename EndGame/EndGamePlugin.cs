using System;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.Plugins;

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
				return "Takes a screenshot of the end game screen.";
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
				// TODO: move this to config/build file
				return new Version(0, 3, 2);
			}
		}

		public void OnButtonPress()
		{
			EndGame.ShowSettings();
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
			EndGame.CloseSettings();
			//EndGame.CloseNoteDialog();
		}

		public void OnUpdate()
		{
		}
	}
}