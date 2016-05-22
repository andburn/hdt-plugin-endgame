using System.Collections.Generic;
using HDT.Plugins.EndGame.Enums;
using Newtonsoft.Json;

namespace HDT.Plugins.EndGame.Models
{
	public class ArchetypeDeck : Deck
	{
		private string _name;

		[JsonProperty("name")]
		public string Name
		{
			get { return _name; }
			set { Set(() => Name, ref _name, value); }
		}

		private ArchetypeStyle _style;

		[JsonProperty("style")]
		public ArchetypeStyle Style
		{
			get { return _style; }
			set { Set(() => Style, ref _style, value); }
		}

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