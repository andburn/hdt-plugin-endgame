using System.Windows;
using System.Windows.Input;

using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Drawing;
using HDT.Plugins.EndGame.Properties;
using System.IO;
using System;

namespace HDT.Plugins.EndGame
{
	public partial class NoteDialog
	{
		private readonly GameStats _game;
		private readonly bool _initialized;
		private Screenshot _screenshot;

		public NoteDialog(GameStats game, List<Screenshot> screenshots)
		{
			InitializeComponent();
			_game = game;
			CheckBoxEnterToSave.IsChecked = Config.Instance.EnterToSaveNote;
			Show();
			Activate();
			TextBoxNote.Focus();

			ListBox_Images.DataContext = screenshots;

			_initialized = true;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			SaveAndClose();
		}

		private void SaveAndClose()
		{
			if (_game != null)
			{
				_game.Note = TextBoxNote.Text;
				DeckStatsList.Save();

				if (Config.Instance.StatsInWindow)
				{
					Logger.WriteLine("refreshing window", "gameshot");
					((DeckStatsControl)Helper.MainWindow.StatsWindow.FindName("StatsControl")).Refresh();
				}
				else
				{
					Logger.WriteLine("refreshing flyout", "gameshot");
					((DeckStatsControl)Helper.MainWindow.FindName("DeckStatsFlyout")).Refresh();
				}

				if (_screenshot != null)
				{
					try
					{
						Logger.WriteLine("saving screenshot", "gameshot");
						var filename = Settings.Default.FilePrefix + _game.EndTime.ToString("dd-MM-yyyy_HHmm") + "_" + _game.OpponentName + "(" + _game.OpponentHero + ").png";
						Logger.WriteLine("filename: " + filename, "gameshot");
						_screenshot.Image.Save(Path.Combine(Settings.Default.OutputDir, filename));
					}
					catch (Exception e)
					{
						Logger.WriteLine(e.Message, "gameshot");
						Logger.WriteLine(Settings.Default.OutputDir, "gameshot");
					}
				}				
			}
			Close();
		}

		private void TextBoxNote_OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && Config.Instance.EnterToSaveNote)
				SaveAndClose();
		}

		private void CheckBoxEnterToSave_OnChecked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;
			Config.Instance.EnterToSaveNote = true;
			Config.Save();
		}

		private void CheckBoxEnterToSave_OnUnchecked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;
			Config.Instance.EnterToSaveNote = false;
			Config.Save();
		}

		private void ListBox_Images_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (ListBox_Images.SelectedItem != null)
			{
				_screenshot = (Screenshot)ListBox_Images.SelectedItem;
			}
		}

	}
}