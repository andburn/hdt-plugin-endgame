using System;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Views;
using Hearthstone_Deck_Tracker.Plugins;

namespace HDT.Plugins.EndGame
{
	public class EndGamePlugin : IPlugin
	{
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
				return null;
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
			new NoteView().Show();
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
		}

		public void OnUpdate()
		{
		}
	}
}