using GameEndShot.Properties;
using Hearthstone_Deck_Tracker;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace HDT.Plugins.GameEndShot.Controls
{
	public partial class PluginSettings : CustomDialog
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
			Helper.MainWindow.HideMetroDialogAsync(this);
		}

		private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
		{
			Helper.MainWindow.HideMetroDialogAsync(this);
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
