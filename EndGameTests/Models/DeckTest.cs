using System.Collections.Generic;
using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestClass]
	public class DeckTest
	{
		private static Deck _deck1;
		private static Deck _deck2;
		private static Deck _deck3;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_deck1 = new Deck();
			_deck1.Cards = new List<Card>()
			{
				new Card("123", "ABC", 1, null),
				new Card("456", "DEF", 2, null)
			};
			_deck2 = new Deck();
			_deck2.Cards = new List<Card>()
			{
				new Card("123", "ABC", 2, null),
				new Card("789", "XYZ", 2, null)
			};
			_deck3 = new Deck();
			_deck3.Cards = new List<Card>()
			{
				new Card("789", "XYZ", 1, null),
				new Card("012", "RST", 1, null)
			};
		}

		[TestMethod]
		public void SimilarityWithSelf()
		{
			Assert.AreEqual(1.0f, _deck1.Similarity(_deck1));
		}

		[TestMethod]
		public void SimilarityWithDifferent()
		{
			Assert.AreEqual(0.0f, _deck1.Similarity(_deck3));
		}

		[TestMethod]
		public void SimilarityWithPartial()
		{
			Assert.AreEqual(0.5f, _deck1.Similarity(_deck2));
		}

		[TestMethod]
		public void SimilarityWithEmpty()
		{
			Assert.AreEqual(0.0f, _deck1.Similarity(new Deck()));
		}
	}
}