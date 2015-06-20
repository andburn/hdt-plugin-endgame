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

		public PluginSettings()
		{
			InitializeComponent();
			LoadSettings();
		}

		private void BtnDefaultDirectory_Click(object sender, RoutedEventArgs e)
		{
			FolderBrowserDialog fdlg = new FolderBrowserDialog();
			fdlg.RootFolder = Environment.SpecialFolder.MyComputer;
			DialogResult result = fdlg.ShowDialog();
			if (result == DialogResult.OK)
			{
				_defaultPath = fdlg.SelectedPath;
			}
		}

		private void BtnSave_OnClick(object sender, RoutedEventArgs e)
		{
			SaveSettings();
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
		}

		private void LoadSettings()
		{
			_defaultPath = Settings.Default.OutputDir;

			TextBox_Prefix.Text = Settings.Default.FilePrefix;
			TextBox_Delay.Text = Settings.Default.Delay.ToString();
			CheckBox_Advanced.IsChecked = Settings.Default.UseAdvancedShot;
		}

		private void SaveSettings()
		{
			if (!String.IsNullOrEmpty(_defaultPath))
			{
				Settings.Default.OutputDir = _defaultPath;
			}

			Settings.Default.FilePrefix = TextBox_Prefix.Text;
			int delay = 0;
			bool result = int.TryParse(TextBox_Delay.Text, out delay);
			if (result)
				Settings.Default.Delay = delay;

			Settings.Default.UseAdvancedShot = (bool)CheckBox_Advanced.IsChecked;

			Settings.Default.Save();
		}
	}
}
