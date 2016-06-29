namespace HDT.Plugins.EndGame.Models
{
	public class ArchetypeDeck : Deck
	{
		public string Name { get; set; }

		public ArchetypeDeck()
			: base()
		{
		}

		public ArchetypeDeck(string name, string format, string klass)
			: base(format, klass)
		{
			Name = name;
		}
	}
}