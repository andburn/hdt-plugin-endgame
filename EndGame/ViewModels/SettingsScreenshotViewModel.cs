using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsScreenshotViewModel : ViewModelBase
	{
		private Settings _settings;

		public Settings Settings
		{
			get { return _settings; }
			set { Set(() => Settings, ref _settings, value); }
		}

		private string _patternPreview;

		public string PatternPreview
		{
			get { return _patternPreview; }
			set { Set(() => PatternPreview, ref _patternPreview, value); }
		}

		public RelayCommand<string> PatternChangedCommand { get; private set; }
		public RelayCommand ChooseDirCommand { get; private set; }

		public SettingsScreenshotViewModel()
		{
			Settings = Settings.Default;

			Settings.PropertyChanged += Settings_PropertyChanged;

			UpdatePattern(Settings.FileNamePattern);

			PatternChangedCommand = new RelayCommand<string>(x => UpdatePattern(x));
			ChooseDirCommand = new RelayCommand(() => ChooseOuputDir());
		}

		private void ChooseOuputDir()
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = true;
			if (Directory.Exists(Settings.OutputDir))
				dialog.DefaultDirectory = Settings.OutputDir;

			CommonFileDialogResult result = dialog.ShowDialog();
			if (result == CommonFileDialogResult.Ok)
				Settings.OutputDir = dialog.FileName;
		}

		private string OpenBrowseDialog(bool folderSelect = false, string filter = null)
		{
			var dialog = new CommonOpenFileDialog();
			dialog.IsFolderPicker = folderSelect;

			if (!string.IsNullOrEmpty(filter))
			{
				var fs = filter.Split(new char[] { ';' }, 2);
				if (fs.Length >= 2)
					dialog.Filters.Add(new CommonFileDialogFilter(fs[0], fs[1]));
			}

			CommonFileDialogResult result = dialog.ShowDialog();

			if (result == CommonFileDialogResult.Ok)
				return dialog.FileName;
			else
				return null;
		}

		private void UpdatePattern(string x)
		{
			NamingPattern np = null;
			var success = NamingPattern.TryParse(x, out np);
			if (success)
				PatternPreview = np.Apply("Mage", "Druid", "Player", "Opponent");
			else
				PatternPreview = "the pattern is invalid";
		}

		private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Settings.Save();
		}
	}
}