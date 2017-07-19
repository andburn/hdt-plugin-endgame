using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;

namespace HDT.Plugins.EndGame.ViewModels
{
	public class StatsViewModel : ViewModelBase
	{
		private IEnumerable<Deck> _decks;

		public IEnumerable<Deck> Decks
		{
			get { return _decks; }
			set { Set(() => Decks, ref _decks, value); }
		}

		private IEnumerable<GameMode> _gameModes;

		public IEnumerable<GameMode> GameModes
		{
			get { return _gameModes; }
			set { Set(() => GameModes, ref _gameModes, value); }
		}

		private IEnumerable<GameFormat> _gameFormats;

		public IEnumerable<GameFormat> GameFormats
		{
			get { return _gameFormats; }
			set { Set(() => GameFormats, ref _gameFormats, value); }
		}

		private IEnumerable<TimeFrame> _timePeriods;

		public IEnumerable<TimeFrame> TimePeriods
		{
			get { return _timePeriods; }
			set { Set(() => TimePeriods, ref _timePeriods, value); }
		}

		private IEnumerable<Region> _regions;

		public IEnumerable<Region> Regions
		{
			get { return _regions; }
			set { Set(() => Regions, ref _regions, value); }
		}

		private GameMode _selectedGameMode;

		public GameMode SelectedGameMode
		{
			get { return _selectedGameMode; }
			set { Set(() => SelectedGameMode, ref _selectedGameMode, value); }
		}

		private GameFormat _selectedGameFormat;

		public GameFormat SelectedGameFormat
		{
			get { return _selectedGameFormat; }
			set { Set(() => SelectedGameFormat, ref _selectedGameFormat, value); }
		}

		private TimeFrame _selectedTimeFrame;

		public TimeFrame SelectedTimeFrame
		{
			get { return _selectedTimeFrame; }
			set { Set(() => SelectedTimeFrame, ref _selectedTimeFrame, value); }
		}

		private Region _selectedRegion;

		public Region SelectedRegion
		{
			get { return _selectedRegion; }
			set { Set(() => SelectedRegion, ref _selectedRegion, value); }
		}

		private Deck _selectedDeck;

		public Deck SelectedDeck
		{
			get { return _selectedDeck; }
			set { Set(() => SelectedDeck, ref _selectedDeck, value); }
		}

		public StatsViewModel()
		{			
			// initialize selection lists
			GameModes = Enum.GetValues(typeof(GameMode)).OfType<GameMode>();
			GameFormats = Enum.GetValues(typeof(GameFormat)).OfType<GameFormat>();
			TimePeriods = Enum.GetValues(typeof(TimeFrame)).OfType<TimeFrame>();
			Regions = Enum.GetValues(typeof(Region)).OfType<Region>().Where(x => x != Region.UNKNOWN);
			Decks = EndGame.Data.GetAllDecks();
			// set default selections
			SelectedGameMode = GameMode.ALL;
			SelectedGameFormat = GameFormat.ANY;
			SelectedRegion = Region.US;
			SelectedTimeFrame = TimeFrame.ALL;
		}
	}
}
