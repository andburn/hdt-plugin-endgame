using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;

namespace HDT.Plugins.EndGame.Utilities
{
	public class DesignerRepository : ITrackerRepository
	{
		public List<ArchetypeDeck> GetAllArchetypeDecks()
		{
			return new List<ArchetypeDeck>() {
				new ArchetypeDeck("Control", Klass.Warrior, true),
				new ArchetypeDeck("Patron", Klass.Warrior, true),
				new ArchetypeDeck("Warrior", Klass.Warrior, true),
				new ArchetypeDeck("Zoolock", Klass.Warlock, true)
			};
		}

		public string GetGameMode()
		{
			return "ranked";
		}

		public string GetGameNote()
		{
			return "I'm a note";
		}

		public Deck GetOpponentDeck()
		{
			StreamResourceInfo sri = Application.GetResourceStream(
				new Uri("pack://application:,,,/EndGame;component/Resources/card_sample.bmp"));
			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = sri.Stream;
			bmp.EndInit();

			var deck = new Deck(Klass.Druid, true);
			deck.Cards = new List<Card>() {
				new Card("OG_280", "C'thun", 1, new ImageBrush(bmp)),
				new Card("OG_280", "C'thun", 1, new ImageBrush(bmp)),
				new Card("OG_280", "C'thun", 1, new ImageBrush(bmp))
			};
			return deck;
		}

		public void AddDeck(Deck deck)
		{
		}

		public void AddDeck(string name, string playerClass, string cards, params string[] tags)
		{
		}

		public void DeleteAllDecksWithTag(string tag)
		{
		}

		public void UpdateGameNote(string text)
		{
		}
	}
}