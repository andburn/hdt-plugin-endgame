using System.Windows.Media;

namespace HDT.Plugins.EndGame.Models
{
	public class Card
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
		public ImageBrush Image { get; set; }

		public Card()
		{
		}

		public Card(string id, string name, int count, ImageBrush image)
		{
			Id = id;
			Name = name;
			Count = count;
			Image = image;
		}

		// two cards are equal once the ids are the same
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}

			Card c = obj as Card;
			if (c == null)
			{
				return false;
			}

			return Id == c.Id;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}