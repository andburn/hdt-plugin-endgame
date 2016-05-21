using System.Collections.Generic;
using HDT.Plugins.EndGame.Archetype;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Archetype
{
	[TestClass]
	public class PlayedDeckTest
	{
		private Deck _archDeck;
		private PlayedDeck _playDeck;

		[TestInitialize]
		public void Setup()
		{
			_archDeck = new ArchetypeDeck(
				"Control Warrior",
				PlayerClass.WARRIOR,
				GameFormat.STANDARD,
				ArchetypeStyles.CONTROL,
				new List<Card>()
			);
			_playDeck = new PlayedDeck("Warrior", Format.Standard, 7,
				new List<TrackedCard>() {
					new TrackedCard("AB_123", 2),
					new TrackedCard("AB_124", 1),
					new TrackedCard("AB_125", 1),
					new TrackedCard("AB_126", 1),
					new TrackedCard("AB_127", 2),
					new TrackedCard("AB_128", 1)
				}
			);
		}

		[TestMethod]
		public void DefaultConstructorHasDefaultProps()
		{
			var deck = new PlayedDeck();
			Assert.AreEqual(0, deck.Turns);
			Assert.AreEqual(PlayerClass.ANY, deck.Klass);
			Assert.AreEqual(GameFormat.ANY, deck.Format);
			Assert.IsTrue(deck.Cards.Count == 0);
		}

		[TestMethod]
		public void ParamConstructorAssignsProps()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, _playDeck.Klass);
			Assert.AreEqual(GameFormat.STANDARD, _playDeck.Format);
			Assert.AreEqual(7, _playDeck.Turns);
			Assert.AreEqual(6, _playDeck.Cards.Count);
		}

		[TestMethod]
		public void SimilarityArchOneFoundOne()
		{
			_archDeck.Cards.Add(new SingleCard("AB_125"));
			Assert.AreEqual(1.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityArchOneFoundZero()
		{
			_archDeck.Cards.Add(new SingleCard("AB_789"));
			Assert.AreEqual(0.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityArchTwoFoundTwo()
		{
			_archDeck.Cards.Add(new SingleCard("AB_124"));
			_archDeck.Cards.Add(new SingleCard("AB_128"));
			Assert.AreEqual(1.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityArchTwoFoundOne()
		{
			_archDeck.Cards.Add(new SingleCard("AB_124"));
			_archDeck.Cards.Add(new SingleCard("AB_789"));
			Assert.AreEqual(0.5, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityArchTwoFoundZero()
		{
			_archDeck.Cards.Add(new SingleCard("AB_456"));
			_archDeck.Cards.Add(new SingleCard("AB_789"));
			Assert.AreEqual(0.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityDifferentKlassShouldBeZero()
		{
			_archDeck.Cards.Add(new SingleCard("AB_125"));
			_archDeck.Klass = PlayerClass.HUNTER;
			Assert.AreEqual(0.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityDifferentFormatsShouldBeZero()
		{
			_archDeck.Cards.Add(new SingleCard("AB_125"));
			_archDeck.Format = GameFormat.WILD;
			Assert.AreEqual(0.0, _playDeck.Similarity(_archDeck));
		}

		[TestMethod]
		public void SimilarityArchFormatAll()
		{
			_archDeck.Cards.Add(new SingleCard("AB_125"));
			_archDeck.Format = GameFormat.ANY;
			Assert.AreEqual(1.0, _playDeck.Similarity(_archDeck));
		}
	}
}