namespace HDT.Plugins.EndGame.Models
{
	public enum Klass
	{
		Druid,
		Hunter,
		Mage,
		Paladin,
		Priest,
		Rogue,
		Shaman,
		Warlock,
		Warrior,
		Any
	}

	public static class KlassKonverter
	{
		public static Klass FromString(string s)
		{
			switch (s?.ToLowerInvariant())
			{
				case "druid": return Klass.Druid;
				case "hunter": return Klass.Hunter;
				case "mage": return Klass.Mage;
				case "paladin": return Klass.Paladin;
				case "priest": return Klass.Priest;
				case "rogue": return Klass.Rogue;
				case "shaman": return Klass.Shaman;
				case "warlock": return Klass.Warlock;
				case "warrior": return Klass.Warrior;
				default: return Klass.Any;
			}
		}
	}
}