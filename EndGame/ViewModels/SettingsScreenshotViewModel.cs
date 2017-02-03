using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Utilities;
using Ookii.Dialogs.Wpf;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class SettingsScreenshotViewModel : ViewModelBase
	{
		public string OutputDir
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "OutputDir").Value;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "OutputDir", value);
				RaisePropertyChanged("OutputDir");
			}
		}

		public int Delay
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "Delay").Int;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "Delay", value);
				RaisePropertyChanged("Delay");
			}
		}

		public bool UseAdvancedShot
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "UseAdvancedShot").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "UseAdvancedShot", value);
				RaisePropertyChanged("UseAdvancedShot");
			}
		}

		public int NumberOfImages
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "NumberOfImages").Int;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "NumberOfImages", value);
				RaisePropertyChanged("NumberOfImages");
			}
		}

		public bool WasNoteDialogOn
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "WasNoteDialogOn").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "WasNoteDialogOn", value);
				RaisePropertyChanged("WasNoteDialogOn");
			}
		}

		public bool WasNoteDialogDelayed
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "WasNoteDialogDelayed").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "WasNoteDialogDelayed", value);
				RaisePropertyChanged("WasNoteDialogDelayed");
			}
		}

		public bool WasNoteEnterChecked
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "WasNoteEnterChecked").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "WasNoteEnterChecked", value);
				RaisePropertyChanged("WasNoteEnterChecked");
			}
		}

		public int DelayBetweenShots
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "DelayBetweenShots").Int;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "DelayBetweenShots", value);
				RaisePropertyChanged("DelayBetweenShots");
			}
		}

		public bool RecordArena
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordArena").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordArena", value);
				RaisePropertyChanged("RecordArena");
			}
		}

		public bool RecordBrawl
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordBrawl").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordBrawl", value);
				RaisePropertyChanged("RecordBrawl");
			}
		}

		public bool RecordCasual
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordCasual").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordCasual", value);
				RaisePropertyChanged("RecordCasual");
			}
		}

		public bool RecordFriendly
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordFriendly").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordFriendly", value);
				RaisePropertyChanged("RecordFriendly");
			}
		}

		public bool RecordOther
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordOther").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordOther", value);
				RaisePropertyChanged("RecordOther");
			}
		}

		public bool RecordPractice
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordPractice").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordPractice", value);
				RaisePropertyChanged("RecordPractice");
			}
		}

		public bool RecordRanked
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "RecordRanked").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "RecordRanked", value);
				RaisePropertyChanged("RecordRanked");
			}
		}

		public string FileNamePattern
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "FileNamePattern").Value;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "FileNamePattern", value);
				RaisePropertyChanged("FileNamePattern");
			}
		}

		public bool ScreenshotEnabled
		{
			get
			{
				return EndGame.Settings.Get("ScreenShot", "ScreenshotEnabled").Bool;
			}
			set
			{
				EndGame.Settings.Set("ScreenShot", "ScreenshotEnabled", value);
				RaisePropertyChanged("ScreenshotEnabled");
			}
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
			UpdatePattern(FileNamePattern);

			PatternChangedCommand = new RelayCommand<string>(x => UpdatePattern(x));
			ChooseDirCommand = new RelayCommand(() => ChooseOuputDir());
		}

		private void ChooseOuputDir()
		{
			VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
			dialog.Description = "Select a folder";
			dialog.UseDescriptionForTitle = true;

			// set initial directory to setting if exists
			var current = EndGame.Settings.Get("ScreenShot", "OutputDir").Value;
			if (Directory.Exists(current))
				dialog.SelectedPath = current;

			if ((bool)dialog.ShowDialog())
			{
				EndGame.Settings.Set("ScreenShot", "OutputDir", dialog.SelectedPath);
				RaisePropertyChanged("OutputDir");
			}
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
	}
}