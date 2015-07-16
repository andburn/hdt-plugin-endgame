using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using HDT.Plugins.EndGame.Properties;
using Hearthstone_Deck_Tracker;

namespace HDT.Plugins.EndGame.Controls
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

		private void LoadSettings()
		{
			_defaultPath = Settings.Default.OutputDir;

			TextBox_Prefix.Text = Settings.Default.FilePrefix;
			TextBox_Delay.Text = Settings.Default.Delay.ToString();
			CheckBox_Advanced.IsChecked = Settings.Default.UseAdvancedShot;
			Slider_NumShots.Value = Settings.Default.NumberOfImages;
			TextBox_DelayBetween.Text = Settings.Default.DelayBetweenShots.ToString();

			if(Settings.Default.UseAdvancedShot)
			{
				AdvancedOptionsOn(true);
			}
		}

		private void AdvancedOptionsOn(bool on = false)
		{
			TextBox_Prefix.IsEnabled = on;
			BtnDefaultDirectory.IsEnabled = on;
			Slider_NumShots.IsEnabled = on;
			TextBox_NumShots.IsEnabled = on;
			TextBox_DelayBetween.IsEnabled = on;
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

		private void CheckBox_Advanced_Checked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;

			// disable the default note dialog
			Config.Instance.ShowNoteDialogAfterGame = false;
			Config.Save();

			Settings.Default.UseAdvancedShot = true;
			Settings.Default.Save();

			AdvancedOptionsOn(true);
		}

		private void CheckBox_Advanced_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!_initialized)
				return;

			// reset the default dialog to its previous state
			// TODO: unloading and loading the plugin will mess this up?
			EndGamePlugin.RestoreDefaultNoteSettings();

			Settings.Default.UseAdvancedShot = false;
			Settings.Default.Save();

			AdvancedOptionsOn(false);
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
			else
			{
				TextBox_Delay.Text = Settings.Default.Delay.ToString();
			}
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

		private void TextBox_DelayBetween_TextChanged(object sender, TextChangedEventArgs e)
		{
			if(!_initialized)
				return;

			int delay = 0;
			bool result = int.TryParse(TextBox_DelayBetween.Text, out delay);
			if(result)
			{
				Settings.Default.DelayBetweenShots = delay;
				Settings.Default.Save();
			}
			else
			{
				TextBox_Delay.Text = Settings.Default.DelayBetweenShots.ToString();
			}
		}
	}
}
