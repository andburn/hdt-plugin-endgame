using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.Common.Util;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Utilities
{
	public class DesignerRepository : IDataRepository
	{
		public List<ArchetypeDeck> GetAllArchetypeDecks()
		{
			return new List<ArchetypeDeck>() {
				new ArchetypeDeck("Control", PlayerClass.WARRIOR, true),
				new ArchetypeDeck("Patron", PlayerClass.WARRIOR, true),
				new ArchetypeDeck("Warrior", PlayerClass.WARRIOR, true),
				new ArchetypeDeck("Zoolock", PlayerClass.WARRIOR, true)
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

		public Deck GetOpponentDeck(bool live)
		{
			StreamResourceInfo sri = Application.GetResourceStream(
				new Uri("pack://application:,,,/EndGame;component/Resources/card_sample.bmp"));
			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = sri.Stream;
			bmp.EndInit();

			var deck = new Deck(PlayerClass.DRUID, true);
			var drawingGroup = new DrawingGroup();
			drawingGroup.Children.Add(new ImageDrawing(bmp, new Rect(0, 0, 217, 34)));

			var img = new DrawingBrush(new DrawingImage(drawingGroup).Drawing);
			deck.Cards = new List<Card>() {
				new Card("OG_280", "C'thun", 1, img),
				new Card("OG_280", "C'thun", 1, img),
				new Card("OG_280", "C'thun", 1, img)
			};
			return deck;
		}

		public void AddDeck(Deck deck)
		{
		}

		public void AddDeck(string name, string playerClass, string cards, bool archive, params string[] tags)
		{
		}

		public void DeleteAllDecksWithTag(string tag)
		{
		}

		public void UpdateGameNote(string text)
		{
		}

		public bool IsInMenu()
		{
			return false;
		}

		public List<Game> GetAllGames()
		{
			return new List<Game>();
		}

		public void AddGames(List<Game> games)
		{
		}

		public void UpdateGames(List<Game> games)
		{
		}

		public List<Deck> GetAllDecks()
		{
			return new List<Deck>();
		}

		public List<Deck> GetAllDecksWithTag(string tag)
		{
			return new List<Deck>();
		}
	}
}