using GalaSoft.MvvmLight;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.EndGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HDT.Plugins.EndGame.ViewModels
{
    public class StatsViewModel : ViewModelBase
    {
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
            set { _selectedClass = value; }
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
            set { Set(() => SelectedDeck, ref _selectedDeck, value); Update(value); }
        }

        public StatsViewModel()
        {
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
            SelectedDeck = Decks.First();
            SelectedClass = PlayerClass.ALL;

            PropertyChanged += StatsViewModel_PropertyChanged;
        }

        private void Update(Deck deck)
        {
            EndGame.Logger.Info("Updating stats");
            Stats.Clear();
            int wins = 0;
            int losses = 0;
            foreach (var s in ViewModelHelper.GetArchetypeStats(EndGame.Data, deck))
            {
                EndGame.Logger.Info("Adding " + s.ToString());
                Stats.Add(s);
                wins += s.TotalWins;
                losses += s.TotalLosses;
            }
            TotalWins = wins;
            TotalLosses = losses;
        }

        private void StatsViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDeck")
                EndGame.Logger.Info("Selected " + SelectedDeck.Name);
        }
    }
}