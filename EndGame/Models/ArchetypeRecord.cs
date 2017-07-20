using System;
using GalaSoft.MvvmLight;
using HDT.Plugins.Common.Enums;

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

		private float CalcWinRate()
		{
			var total = (float)(TotalWins + TotalLosses);
			if (total <= 0)
				return 0;
			return TotalWins / total * 100;
		}

		public override string ToString()
		{
			return $"{Name} ({Klass}) {TotalWins}-{TotalLosses} ({WinRateText})";
		}
	}
}