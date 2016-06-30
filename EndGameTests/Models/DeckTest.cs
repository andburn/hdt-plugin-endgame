using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestClass]
	public class DeckTest
	{
		private static Deck _deck1 = new Deck();
		private static Deck _deck2 = new Deck();
		private static Deck _deck3 = new Deck();
		private static Deck _deck4 = new Deck();
		private static Deck _empty = new Deck();

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_deck1.Cards = TestHelper.CreateCards("AB_123:1;AB_456:1");
			_deck2.Cards = TestHelper.CreateCards("AB_789:1;AB_456:1;AB_123:1");
			_deck3.Cards = TestHelper.CreateCards("NT_010:1;NT_002:1;AB_789:1");
			_deck4.Cards = TestHelper.CreateCards("AB_123:2;NT_001:1");
		}

		[TestMethod]
		public void SimilarityWithSelf()
		{
			Assert.AreEqual(1.0f, _deck1.Similarity(_deck1));
		}

		[TestMethod]
		public void SimilarityWithNoneInCommon()
		{
			Assert.AreEqual(0.0f, _deck1.Similarity(_deck3));
		}

		[TestMethod]
		public void SimilarityWithSingleInCommon()
		{
			Assert.AreEqual(0.2f, _deck2.Similarity(_deck3));
			Assert.AreEqual(0.2f, _deck3.Similarity(_deck2));
		}

		[TestMethod]
		public void SimilarityWithTwoInCommon()
		{
			Assert.AreEqual(0.67f, _deck1.Similarity(_deck2));
			Assert.AreEqual(0.67f, _deck2.Similarity(_deck1));
		}

		[TestMethod]
		public void SimilarityWithOneInCommonAndCountTwo()
		{
			Assert.AreEqual(0.25f, _deck1.Similarity(_deck4));
			Assert.AreEqual(0.25f, _deck4.Similarity(_deck1));
		}

		[TestMethod]
		public void SimilarityWithEmpty()
		{
			Assert.AreEqual(0.0f, _deck1.Similarity(_empty));
			Assert.AreEqual(0.0f, _empty.Similarity(_deck1));
		}

		[TestMethod]
		public void SimilarityWithNull()
		{
			Assert.AreEqual(0.0f, _deck1.Similarity(null));
		}

		[TestMethod]
		public void SimilarityEmptyAndEmpty()
		{
			Assert.AreEqual(1.0f, _empty.Similarity(_empty));
		}
	}
}