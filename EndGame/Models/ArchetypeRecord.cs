using GalaSoft.MvvmLight;
using HDT.Plugins.Common.Enums;
using System;

namespace HDT.Plugins.EndGame.Models
{
    public class ArchetypeRecord : ObservableObject
    {
        public static readonly string DefaultName = "Unknown";

        public string Name { get; set; }
        public PlayerClass Klass { get; set; }
        public int TotalWins { get; set; }
        public int TotalLosses { get; set; }

        public float WinRate
        {
            get { return CalcWinRate(); }
        }

        public string WinRateText
        {
            get { return ((int)Math.Round(WinRate)).ToString() + "%"; }
        }

        public ArchetypeRecord()
        {
            Name = DefaultName;
            Klass = PlayerClass.ALL;
        }

        public ArchetypeRecord(string name, PlayerClass klass)
        {
            Name = name;
            Klass = klass;
        }

        public void Update(GameResult result)
        {
            switch (result)
            {
                case GameResult.WIN:
                    TotalWins++;
                    break;

                case GameResult.LOSS:
                    TotalLosses++;
                    break;

                default:
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            var record = obj as ArchetypeRecord;
            if (record == null)
                return false;

            return Name.Equals(record.Name) && Klass == record.Klass;
        }

        public override int GetHashCode()
        {
            return $"{Klass}.{Name}".GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}.{Klass}";
        }

        private float CalcWinRate()
        {
            var total = (float)(TotalWins + TotalLosses);
            if (total <= 0)
                return 0;
            return TotalWins / total * 100;
        }
    }
}