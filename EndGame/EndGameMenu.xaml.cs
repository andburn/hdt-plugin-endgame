using System.Windows;
using System.Windows.Controls;

namespace HDT.Plugins.EndGame
{
	public partial class EndGameMenu : MenuItem
	{
		public EndGameMenu()
		{
			InitializeComponent();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
		}

		private void Menu_Settings_Click(object sender, RoutedEventArgs e)
		{
			EndGame.ShowSettings();
		}

		private void Menu_ImportDecks_Click(object sender, RoutedEventArgs e)
		{
			EndGame.ImportMetaDecks();
		}
	}
}