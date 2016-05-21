using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HDT.Plugins.EndGame.Archetype;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Archetype
{
	[TestClass]
	public class ArchetypeManagerTest
	{
		private const int DEFAULT_STYLE_COUNT = 4;

		private ArchetypeManager _manager = ArchetypeManager.Instance;

		private string _json = "[{\"Name\":\"One\",\"Style\":{\"Name\":\"Control\",\"Style\":0},\"Id\":\"d493e262-68ed-4d37-93ad-d81e8cef9b21\",\"Klass\":3,\"Format\":1,\"Cards\":[{\"Id\":\"AB_123\",\"Count\":1},{\"Id\":\"AB_124\",\"Count\":1},{\"Id\":\"AB_125\",\"Count\":1}]}]";

		private ArchetypeDeck _sample = new ArchetypeDeck(
			"Face ",
			PlayerClass.HUNTER,
			GameFormat.STANDARD,
			ArchetypeStyles.AGGRO,
			new List<Card>() {
				new SingleCard("AB_123"),
				new SingleCard("AB_456")
			}
		);

		private List<ArchetypeDeck> _decks = new List<ArchetypeDeck>() {
			new ArchetypeDeck("One", PlayerClass.HUNTER, GameFormat.STANDARD, ArchetypeStyles.CONTROL,
				new List<Card>() {
					new SingleCard("AB_123"),
					new SingleCard("AB_124"),
					new SingleCard("AB_125")
				}
			)
		};

		[TestInitialize]
		public void Setup()
		{
			_manager.Reset();
		}

		[TestMethod]
		public void AddDeck()
		{
			_manager.AddDeck(new ArchetypeDeck());
			Assert.AreEqual(1, _manager.Decks.Count);
		}

		[TestMethod]
		public void DontAddDuplicateDeck()
		{
			_manager.AddDeck(_sample);
			_manager.AddDeck(new ArchetypeDeck(
				"Face ",
				PlayerClass.HUNTER,
				GameFormat.STANDARD,
				ArchetypeStyles.AGGRO,
				new List<Card>() {
					new SingleCard("AB_123"),
					new SingleCard("AB_456")
				}
			));
			Assert.AreEqual(1, _manager.Decks.Count);
		}

		[TestMethod]
		public void RemoveDeck()
		{
			_manager.AddDeck(_sample);
			Assert.AreEqual(1, _manager.Decks.Count);
			_manager.RemoveDeck(_sample);
			Assert.AreEqual(0, _manager.Decks.Count);
		}

		[TestMethod]
		public void RemoveMatchingDecks()
		{
			_manager.AddDeck(_sample);
			_manager.AddDeck(new ArchetypeDeck(
				"rAmp",
				PlayerClass.DRUID,
				GameFormat.WILD,
				ArchetypeStyles.COMBO,
				new List<Card>() {
					new SingleCard("AB_987"),
					new SingleCard("AB_321")
				}
			));
			Assert.AreEqual(2, _manager.Decks.Count);
			_manager.RemoveDeck(new ArchetypeDeck(
				"rAmp",
				PlayerClass.DRUID,
				GameFormat.WILD,
				ArchetypeStyles.COMBO,
				new List<Card>() {
					new SingleCard("AB_987"),
					new SingleCard("AB_321")
				}
			));
			Assert.AreEqual(1, _manager.Decks.Count);
		}

		[TestMethod]
		public void FindBestMatch()
		{
			_manager.AddDeck(_sample);
			_manager.AddDeck(new ArchetypeDeck(
				"rAmp",
				PlayerClass.DRUID,
				GameFormat.WILD,
				ArchetypeStyles.COMBO,
				new List<Card>() {
					new SingleCard("AB_987"),
					new SingleCard("AB_321")
				}
			));
			var deck = new PlayedDeck("Druid", Format.Wild, 5, new List<TrackedCard>() {
				new TrackedCard("OG_129", 1),
				new TrackedCard("AT_387", 1),
				new TrackedCard("AB_321", 1),
				new TrackedCard("FP1_298", 1)
			});
			Assert.AreEqual("rAmp", _manager.Find(deck).First().Name);
		}

		[TestMethod]
		public void DefaultDeckStyles()
		{
			Assert.AreEqual(DEFAULT_STYLE_COUNT, _manager.Styles.Count);
		}

		[TestMethod]
		public void AddDeckStyle()
		{
			_manager.AddStyle(new ArchetypeStyle("Mill", PlayStyle.CUSTOM));
			Assert.AreEqual(DEFAULT_STYLE_COUNT + 1, _manager.Styles.Count);
		}

		[TestMethod]
		public void DoNotAddDuplicateDeckStyle()
		{
			_manager.AddStyle(new ArchetypeStyle("Mill", PlayStyle.CUSTOM));
			_manager.AddStyle(new ArchetypeStyle("Mill", PlayStyle.CUSTOM));
			Assert.AreEqual(DEFAULT_STYLE_COUNT + 1, _manager.Styles.Count);
		}

		[TestMethod]
		public void RemoveCustomDeckStyle()
		{
			_manager.AddStyle(new ArchetypeStyle("Mill", PlayStyle.CUSTOM));
			_manager.RemoveStyle(new ArchetypeStyle("Mill", PlayStyle.CUSTOM));
			Assert.AreEqual(DEFAULT_STYLE_COUNT, _manager.Styles.Count);
		}

		[TestMethod]
		public void RemoveDefaultDeckStyleFails()
		{
			_manager.RemoveStyle(ArchetypeStyles.CONTROL);
			Assert.AreEqual(DEFAULT_STYLE_COUNT, _manager.Styles.Count);
		}

		[TestMethod]
		public void StylesPropDoesNotAlterBackingFields()
		{
			_manager.Styles.Add(ArchetypeStyles.CONTROL);
			Assert.AreEqual(DEFAULT_STYLE_COUNT, _manager.Styles.Count);
			_manager.Styles.Clear();
			Assert.AreEqual(DEFAULT_STYLE_COUNT, _manager.Styles.Count);
		}

		[TestMethod]
		public void LoadArcheyptesFromFile()
		{
			_manager.LoadDecks("data/decks.json");
			Assert.AreEqual(1, _manager.Decks.Count);
			Assert.AreEqual(Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21"), _manager.Decks[0].Id);
			Assert.AreEqual("Some Deck", _manager.Decks[0].Name);
			Assert.AreEqual(PlayerClass.HUNTER, _manager.Decks[0].Klass);
			Assert.AreEqual(GameFormat.STANDARD, _manager.Decks[0].Format);
			Assert.AreEqual(3, _manager.Decks[0].Cards.Count);
		}

		[TestMethod]
		public void SaveArcheyptesToFile()
		{
			var deck = _decks.First();
			deck.Id = Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21");
			_manager.AddDeck(deck);
			_manager.SaveDecks("data/saved.json");
			var read = File.ReadAllText("data/saved.json");
			Assert.AreEqual(_json, read);
		}
	}
}