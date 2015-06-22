using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

using GameEndShot.Properties;
using Hearthstone_Deck_Tracker;

namespace HDT.Plugins.GameEndShot.Controls
{
	public partial class PluginSettings : System.Windows.Controls.UserControl
	{
		private string _defaultPath;
		private bool _initialized;

		public PluginSettings()
		{
			InitializeComponent();
			LoadSettings();
			_initialized = true;
		}

		private void BtnDefaultDirectory_Click(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;

			FolderBrowserDialog fdlg = new FolderBrowserDialog();
			fdlg.RootFolder = Environment.SpecialFolder.MyComputer;
			DialogResult result = fdlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				_defaultPath = fdlg.SelectedPath;
				if (!String.IsNullOrEmpty(_defaultPath))
				{
					Settings.Default.OutputDir = _defaultPath;
					Settings.Default.Save();
				}				
			}
		}

		private void LoadSettings()
		{
			_defaultPath = Settings.Default.OutputDir;
			
			TextBox_Prefix.Text = Settings.Default.FilePrefix;
			TextBox_Delay.Text = Settings.Default.Delay.ToString();
			CheckBox_Advanced.IsChecked = Settings.Default.UseAdvancedShot;
			Slider_NumShots.Value = Settings.Default.NumberOfImages;

			if (Settings.Default.UseAdvancedShot)
			{
				TextBox_Prefix.IsEnabled = true;
				BtnDefaultDirectory.IsEnabled = true;
				Slider_NumShots.IsEnabled = true;
				TextBox_NumShots.IsEnabled = true;
			}
		}

		private void CheckBox_Advanced_Checked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;

			// disable the default note dialog
			Config.Instance.ShowNoteDialogAfterGame = false;
			Config.Save();
			// TODO: add default note settings to new note dialog

			Settings.Default.UseAdvancedShot = true;
			Settings.Default.Save();

			TextBox_Prefix.IsEnabled = true;
			BtnDefaultDirectory.IsEnabled = true;
			Slider_NumShots.IsEnabled = true;
			TextBox_NumShots.IsEnabled = true;
		}

		private void CheckBox_Advanced_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;

			// reset the default dialog to its previous state
			GameEndShotPlugin.RestoreDefaultNoteSettings();

			Settings.Default.UseAdvancedShot = false;
			Settings.Default.Save();

			TextBox_Prefix.IsEnabled = false;
			BtnDefaultDirectory.IsEnabled = false;
			Slider_NumShots.IsEnabled = false;
			TextBox_NumShots.IsEnabled = false;
		}

		private void TextBox_Delay_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_initialized)
				return;

			int delay = 0;
			bool result = int.TryParse(TextBox_Delay.Text, out delay);
			if (result)
			{
				Settings.Default.Delay = delay;
				Settings.Default.Save();
			}
			// TODO: error on parse failure
		}

		private void Slider_NumShots_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (!_initialized)
				return;

			Settings.Default.NumberOfImages = (int)Slider_NumShots.Value;
			Settings.Default.Save();			
		}

		private void TextBox_Prefix_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!_initialized)
				return;

			Settings.Default.FilePrefix = TextBox_Prefix.Text;
			Settings.Default.Save();
		}
	}
}
