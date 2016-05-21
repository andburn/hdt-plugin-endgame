using HDT.Plugins.EndGame.Archetype;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Archetype
{
	[TestClass]
	public class CardTest
	{
		[TestMethod]
		public void DefaultConstructorHasNullProps()
		{
			var card = new Card();
			Assert.AreEqual(0, card.Count);
			Assert.IsNull(card.Id);
		}

		[TestMethod]
		public void ParamConstructorAssignsProps()
		{
			var card = new Card("OG_123", 2);
			Assert.AreEqual(2, card.Count);
			Assert.AreEqual("OG_123", card.Id);
		}

		[TestMethod]
		public void TrackedCardConstructor()
		{
			var card = new Card(new TrackedCard("AT_129", 2));
			Assert.AreEqual("AT_129", card.Id);
			Assert.AreEqual(2, card.Count);
		}

		[TestMethod]
		public void EqualityOverrideCastable()
		{
			Card card = new Card("OG_123", 2);
			object same = new Card("OG_123", 2);
			object other = new Card("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void EqualityOverrideNotCastable()
		{
			Card card = new Card("OG_123", 2);
			object other = "a string";
			object none = null;
			Assert.IsFalse(card.Equals(other));
			Assert.IsFalse(card.Equals(none));
		}

		[TestMethod]
		public void EqualitySameType()
		{
			var card = new Card("OG_123", 2);
			var same = new Card("OG_123", 2);
			var other = new Card("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void EqualityTrackedCard()
		{
			var card = new Card("OG_123", 2);
			var same = new TrackedCard("OG_123", 2);
			var other = new TrackedCard("AT_128", 2);
			Assert.IsTrue(card.Equals(same));
			Assert.IsFalse(card.Equals(other));
		}

		[TestMethod]
		public void TestHashCode()
		{
			var card = new Card("OG_123", 2);
			var same = new Card("OG_123", 2);
			Assert.AreEqual(card.GetHashCode(), same.GetHashCode());
		}

		[TestMethod]
		public void TestCompareTo()
		{
			var one = new Card("OG_123", 2);
			var two = new Card("XY_456", 2);
			var three = new Card("AB_789", 1);
			Assert.AreEqual(0, one.CompareTo(one));
			Assert.AreEqual(1, two.CompareTo(three));
			Assert.AreEqual(-1, three.CompareTo(two));
		}

		[TestMethod]
		public void TestToString()
		{
			var card = new Card("OG_123", 2);
			Assert.AreEqual("OG_123 x2", card.ToString());
		}
	}
}