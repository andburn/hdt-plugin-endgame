using System.Windows.Media;

namespace HDT.Plugins.EndGame.Models
{
	public class Card
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
		public ImageBrush Image { get; set; }

		public Card(string id, string name, int count, ImageBrush image)
		{
			Id = id;
			Name = name;
			Count = count;
			Image = image;
		}
	}
}