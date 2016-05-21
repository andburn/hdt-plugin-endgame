using System;
using System.Collections.Generic;
using HDT.Plugins.EndGame.Archetype;
using HDT.Plugins.EndGame.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Archetype
{
	[TestClass]
	public class DeckTest
	{
		private class ConcreteDeck : Deck
		{
			public ConcreteDeck() : base()
			{
			}

			public ConcreteDeck(PlayerClass klass, GameFormat format, List<Card> cards)
				: base(klass, format, cards)
			{
			}
		}

		[TestMethod]
		public void DefaultConstructorHasDefaultProps()
		{
			var deck = new ConcreteDeck();

			Assert.AreEqual(PlayerClass.ANY, deck.Klass);
			Assert.AreEqual(GameFormat.ANY, deck.Format);
			Assert.AreNotEqual(Guid.Empty, deck.Id);
			Assert.IsTrue(deck.Cards.Count == 0);
		}

		[TestMethod]
		public void ParamConstructorAssignsProps()
		{
			var cards = new List<Card>() {
				new Card("AB_123", 1),
				new Card("AB_456", 2)
			};
			var deck = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, cards);

			Assert.AreEqual(PlayerClass.DRUID, deck.Klass);
			Assert.AreEqual(GameFormat.STANDARD, deck.Format);
			Assert.AreEqual(cards, deck.Cards);
			Assert.AreNotEqual(Guid.Empty, deck.Id);
		}

		[TestMethod]
		public void EqualByReference()
		{
			var deck = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, null);
			Assert.AreEqual(deck, deck);
		}

		[TestMethod]
		public void EqualByValue()
		{
			var deckA = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, null);
			deckA.Id = Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21");
			var deckB = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, null);
			deckB.Id = Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21");
			Assert.AreEqual(deckA, deckB);
		}

		[TestMethod]
		public void NotEqualIfDifferentId()
		{
			Assert.AreNotEqual(new ConcreteDeck(), new ConcreteDeck());
		}

		[TestMethod]
		public void HashCodeSameIfEqual()
		{
			var deckA = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, null);
			deckA.Id = Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21");
			var deckB = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD, null);
			deckB.Id = Guid.Parse("d493e262-68ed-4d37-93ad-d81e8cef9b21");
			Assert.AreEqual(deckA.GetHashCode(), deckB.GetHashCode());
		}

		[TestMethod]
		public void DecksWithSamePropsMatch()
		{
			var deckA = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD,
				new List<Card>() {
					new Card("AB_123", 1),
					new Card("AB_456", 2)
				}
			);
			var deckB = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD,
				new List<Card>() {
					new Card("AB_123", 1),
					new Card("AB_456", 2)
				}
			);
			Assert.IsTrue(deckA.Matches(deckB));
		}

		[TestMethod]
		public void DecksWithDifferentPropsDoNotMatch()
		{
			var deckA = new ConcreteDeck(PlayerClass.HUNTER, GameFormat.STANDARD,
				new List<Card>() {
					new Card("AB_897", 1)
				}
			);
			var deckB = new ConcreteDeck(PlayerClass.DRUID, GameFormat.STANDARD,
				new List<Card>() {
					new Card("AB_123", 1),
					new Card("AB_456", 2)
				}
			);
			Assert.IsFalse(deckA.Matches(deckB));
		}
	}
}