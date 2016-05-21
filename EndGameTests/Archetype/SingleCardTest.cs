using HDT.Plugins.EndGame.Archetype;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Archetype
{
	[TestClass]
	public class SingleCardTest
	{
		[TestMethod]
		public void ConstructorAssignsCountOne()
		{
			Card card = new SingleCard("OG_123");
			Assert.AreEqual(1, card.Count);
		}

		[TestMethod]
		public void EqualityOverrideCastable()
		{
			Card card = new SingleCard("OG_123", 2);
			object same = new SingleCard("OG_123", 4);
			object other = new SingleCard("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void EqualityOverrideNotCastable()
		{
			Card card = new SingleCard("OG_123", 2);
			object other = "a string";
			object none = null;
			Assert.IsFalse(card.Equals(other));
			Assert.IsFalse(card.Equals(none));
		}

		[TestMethod]
		public void EqualitySameType()
		{
			var card = new SingleCard("OG_123", 2);
			var same = new SingleCard("OG_123", 1);
			var other = new SingleCard("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void EqualityTrackedCard()
		{
			var card = new SingleCard("OG_123", 2);
			var same = new TrackedCard("OG_123", 5);
			var other = new TrackedCard("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void TestHashCode()
		{
			var card = new SingleCard("OG_123", 2);
			var same = new SingleCard("OG_123", 2);
			var other = new SingleCard("OG_123", 1);
			Assert.AreEqual(card.GetHashCode(), same.GetHashCode());
			Assert.AreEqual(card.GetHashCode(), other.GetHashCode());
		}

		[TestMethod]
		public void TestToString()
		{
			var card = new SingleCard("OG_123", 2);
			Assert.AreEqual("OG_123 [x2]", card.ToString());
		}
	}
}