namespace HDT.Plugins.EndGame.Models
{
	public class ArchetypeDeck : Deck
	{
		public string Name { get; set; }

		public ArchetypeDeck()
			: base()
		{
		}

		public ArchetypeDeck(string name, Klass klass, bool standard)
			: base(klass, standard)
		{
			Name = name;
		}
	}
}