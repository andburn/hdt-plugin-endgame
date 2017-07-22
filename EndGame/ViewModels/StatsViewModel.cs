using GalaSoft.MvvmLight;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Utils;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HDT.Plugins.EndGame.ViewModels
{
    public class StatsViewModel : ViewModelBase
    {
        private List<Game> _games;

        public ObservableCollection<ArchetypeRecord> Stats { get; set; }

        private int _totalWins;

        public int TotalWins
        {
            get { return _totalWins; }
            set { Set(() => TotalWins, ref _totalWins, value); }
        }

        private int _totalLosses;

        public int TotalLosses
        {
            get { return _totalLosses; }
            set { Set(() => TotalLosses, ref _totalLosses, value); }
        }

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

        private IEnumerable<TimeFrame> _timeFrames;

        public IEnumerable<TimeFrame> TimeFrames
        {
            get { return _timeFrames; }
            set { Set(() => TimeFrames, ref _timeFrames, value); }
        }

        private IEnumerable<Region> _regions;

        public IEnumerable<Region> Regions
        {
            get { return _regions; }
            set { Set(() => Regions, ref _regions, value); }
        }

        private IEnumerable<PlayerClass> _classes;

        public IEnumerable<PlayerClass> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        private PlayerClass _selectedClass;

        public PlayerClass SelectedClass
        {
            get { return _selectedClass; }
            set
            {
                _selectedClass = value;
                UpdateStats();
            }
        }

        private GameMode _selectedGameMode;

        public GameMode SelectedGameMode
        {
            get { return _selectedGameMode; }
            set
            {
                Set(() => SelectedGameMode, ref _selectedGameMode, value);
                UpdateStats();
            }
        }

        private GameFormat _selectedGameFormat;

        public GameFormat SelectedGameFormat
        {
            get { return _selectedGameFormat; }
            set
            {
                Set(() => SelectedGameFormat, ref _selectedGameFormat, value);
                UpdateStats();
            }
        }

        private TimeFrame _selectedTimeFrame;

        public TimeFrame SelectedTimeFrame
        {
            get { return _selectedTimeFrame; }
            set
            {
                Set(() => SelectedTimeFrame, ref _selectedTimeFrame, value);
                UpdateStats();
            }
        }

        private Region _selectedRegion;

        public Region SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                Set(() => SelectedRegion, ref _selectedRegion, value);
                UpdateStats();
            }
        }

        private int _rankMin;

        public int RankMin
        {
            get { return _rankMin; }
            set
            {
                Set(() => RankMin, ref _rankMin, value);
                UpdateStats();
            }
        }

        private int _rankMax;

        public int RankMax
        {
            get { return _rankMax; }
            set
            {
                Set(() => RankMax, ref _rankMax, value);
                UpdateStats();
            }
        }

        private Deck _selectedDeck;

        public Deck SelectedDeck
        {
            get { return _selectedDeck; }
            set
            {
                Set(() => SelectedDeck, ref _selectedDeck, value);
                UpdateGames();
                UpdateStats();
            }
        }

        public bool IncludeUnknown
        {
            get
            {
                return EndGame.Settings.Get(Strings.IncludeUnknown).Bool;
            }
            set
            {
                EndGame.Settings.Set(Strings.IncludeUnknown, value);
                RaisePropertyChanged(Strings.IncludeUnknown);
                UpdateStats();
            }
        }

        public StatsViewModel()
        {
            _games = new List<Game>();
            Stats = new ObservableCollection<ArchetypeRecord>();
            // initialize selection lists
            GameModes = Enum.GetValues(typeof(GameMode)).OfType<GameMode>();
            GameFormats = Enum.GetValues(typeof(GameFormat)).OfType<GameFormat>();
            TimeFrames = Enum.GetValues(typeof(TimeFrame)).OfType<TimeFrame>();
            Regions = Enum.GetValues(typeof(Region)).OfType<Region>().Where(x => x != Region.UNKNOWN);
            Classes = Enum.GetValues(typeof(PlayerClass)).OfType<PlayerClass>();

            Decks = ViewModelHelper.GetDecksWithArchetypeGames(EndGame.Data);

            // set default selections
            SelectedGameMode = GameMode.RANKED;
            SelectedGameFormat = GameFormat.STANDARD;
            SelectedRegion = Region.US;
            SelectedTimeFrame = TimeFrame.ALL;
            SelectedDeck = Decks.FirstOrDefault();
            SelectedClass = PlayerClass.ALL;

            RankMax = 0;
            RankMin = 25;
        }

        private void UpdateGames()
        {
            if (SelectedDeck != null && SelectedDeck.Id != Guid.Empty)
            {
                _games = EndGame.Data.GetAllGamesWithDeck(SelectedDeck.Id);
            }
        }

        private void UpdateStats()
        {
            int wins = 0;
            int losses = 0;

            if (_games == null || _games.Count == 0)
                UpdateGames();

            Stats.Clear();

            var filter = new GameFilter(null, SelectedRegion, SelectedGameMode, SelectedTimeFrame,
                SelectedGameFormat, PlayerClass.ALL, SelectedClass, RankMin, RankMax);
            var filtered = filter.Apply(_games);
            var stats = ViewModelHelper.GetArchetypeStats(filtered);
            foreach (var s in stats)
            {
                Stats.Add(s);
                wins += s.TotalWins;
                losses += s.TotalLosses;
            }
            TotalWins = wins;
            TotalLosses = losses;
        }
    }
}