using HDT.Plugins.EndGame.Enums;

namespace HDT.Plugins.EndGame.Models
{
	public static class ArchetypeStyles
	{
		public static readonly ArchetypeStyle CONTROL =
			new ArchetypeStyle("Control", PlayStyle.CONTROL);

		public static readonly ArchetypeStyle AGGRO =
			new ArchetypeStyle("Aggro", PlayStyle.AGGRO);

		public static readonly ArchetypeStyle COMBO =
			new ArchetypeStyle("Combo", PlayStyle.COMBO);

		public static readonly ArchetypeStyle MIDRANGE =
			new ArchetypeStyle("MidRange", PlayStyle.MIDRANGE);
	}

	public class ArchetypeStyle
	{
		public string Name { get; private set; }
		public PlayStyle Style { get; private set; }

		public ArchetypeStyle(string name, PlayStyle style)
		{
			Name = name;
			Style = style;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			ArchetypeStyle o = obj as ArchetypeStyle;
			if (o == null)
			{
				return false;
			}

			return o.Name.Equals(Name) && o.Style == Style;
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ (int)Style;
		}

		public override string ToString()
		{
			return $"{Name} ({Style})";
		}
	}
}