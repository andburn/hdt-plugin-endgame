using System;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Utilities;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class BasicNoteViewModel : ViewModelBase
	{
		private ITrackerRepository _repository;
		private IImageCaptureService _cap;
		private ILoggingService _log;

		private string _note;

		public string Note
		{
			get { return _note; }
			set { Set(() => Note, ref _note, value); }
		}

		private bool _hasScreenshots;

		public bool HasScreenshots
		{
			get { return _hasScreenshots; }
			set { Set(() => HasScreenshots, ref _hasScreenshots, value); }
		}

		public ObservableCollection<Screenshot> Screenshots { get; set; }

		public RelayCommand<string> NoteTextChangeCommand { get; private set; }
		public RelayCommand WindowClosingCommand { get; private set; }

		public BasicNoteViewModel()
		{
			HasScreenshots = false;

			if (IsInDesignMode)
			{
				_repository = new DesignerRepository();
				Screenshots = DesignerData.GenerateScreenshots();
				HasScreenshots = true;
			}
			else
			{
				_repository = new TrackerRepository();
			}
			_cap = new TrackerCapture();
			_log = new TrackerLogger();

			Note = _repository.GetGameNote()?.ToString();

			NoteTextChangeCommand = new RelayCommand<string>(x => _repository.UpdateGameNote(x));
			WindowClosingCommand = new RelayCommand(() => Closing());
		}

		public BasicNoteViewModel(ObservableCollection<Screenshot> screenshots)
			: this()
		{
			Screenshots = screenshots;
			HasScreenshots = Screenshots != null && Screenshots.Any();
		}

		private void Closing()
		{
			var screenshot = Screenshots?.FirstOrDefault(s => s.IsSelected);
			if (screenshot != null)
			{
				_log.Debug($"Attempting to save screenshot #{screenshot.Index}");
				try
				{
					_cap.SaveImage(screenshot);
				}
				catch (Exception e)
				{
					_log.Error(e.Message);
				}
			}
			else
			{
				_log.Debug($"No screenshot selected (len={Screenshots?.Count})");
			}
		}
	}
}