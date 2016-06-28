using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace HDT.Plugins.EndGame.Models
{
	public class PlayedDeck : ObservableObject
	{
		public List<Card> Cards { get; set; }
		public string Hero { get; set; }
		public string Format { get; set; }

		public PlayedDeck()
		{
			Cards = new List<Card>();
		}

		public PlayedDeck(string hero, string format, List<Card> cards)
		{
			Cards = cards;
			Hero = hero;
			Format = format;
		}
	}
}