using System;
using System.Windows;
using System.Windows.Controls;
using HDT.Plugins.EndGame.Views;
using Hearthstone_Deck_Tracker.Plugins;

namespace HDT.Plugins.EndGame
{
	public class EndGamePlugin : IPlugin
	{
		private Window _mainWindow;

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
			get
			{
				return null;
			}
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
			_mainWindow = new MainView();
			_mainWindow.Show();
		}

		public void OnLoad()
		{
		}

		public void OnUnload()
		{
			if (_mainWindow != null && _mainWindow.IsVisible)
			{
				_mainWindow.Close();
			}
		}

		public void OnUpdate()
		{
		}
	}
}