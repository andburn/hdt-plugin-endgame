using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using HDT.Plugins.EndGame.Properties;
using HDT.Plugins.EndGame.Screenshot;
using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.Stats;
using Image = HDT.Plugins.EndGame.Screenshot.Image;

namespace HDT.Plugins.EndGame
{
	public partial class NoteDialog
	{
		private readonly GameStats _game;
		private readonly bool _initialized;
		private Image _screenshot;

		public NoteDialog(GameStats game, List<Image> screenshots)
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
			Capture.SaveImage(_game, _screenshot, TextBoxNote.Text);			
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
				_screenshot = (Image)ListBox_Images.SelectedItem;
			}
		}

	}
}