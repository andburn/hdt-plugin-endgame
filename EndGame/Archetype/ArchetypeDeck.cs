using System.Collections.Generic;
using HDT.Plugins.EndGame.Enums;

namespace HDT.Plugins.EndGame.Archetype
{
	public class ArchetypeDeck : Deck
	{
		public string Name { get; set; }
		public ArchetypeStyle Style { get; set; }

		public ArchetypeDeck()
			: base()
		{
		}

		public ArchetypeDeck(string name, PlayerClass klass, GameFormat format, ArchetypeStyle style, List<Card> cards)
			: base(klass, format, cards)
		{
			Name = name;
			Style = style;
		}

		public override string ToString()
		{
			return Name;
		}

		public string ToNoteString()
		{
			return $"{Name} : {Klass.ToString()}.{Style.Style}";
		}
	}
}