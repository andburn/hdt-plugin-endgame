namespace HDT.Plugins.EndGame.Models
{
	public class ArchetypeDeck
	{
		public string Hero { get; set; }
		public string Name { get; set; }

		public ArchetypeDeck(string name, string hero)
		{
			Name = name;
			Hero = hero;
		}
	}
}