namespace HDT.Plugins.EndGame.Services
{
	public class UpdateResult
	{
		public string StandardPrevious { get; set; }
		public string WildPrevious { get; set; }
		public string StandardLatest { get; set; }
		public string WildLatest { get; set; }

		public UpdateResult()
		{
		}

		public UpdateResult(string stdPrev, string stdNew, string wildPrev, string wildNew)
		{
			StandardPrevious = stdPrev;
			StandardLatest = stdNew;
			WildPrevious = wildPrev;
			WildLatest = wildNew;
		}

		public bool HasUpdates(bool includeWild = true)
		{
			if (string.IsNullOrEmpty(StandardLatest) || string.IsNullOrEmpty(WildLatest))
				return false;

			return StandardLatest.CompareTo(StandardPrevious) >= 1
				|| (includeWild && WildLatest.CompareTo(WildPrevious) >= 1);
		}
	}
}