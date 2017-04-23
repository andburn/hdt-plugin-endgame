using System.Collections.Generic;
using HDT.Plugins.Common.Models;
using HDT.Plugins.EndGame.Models;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class ArchetypeDeckTest
	{
		[Test]
		public void Similarity_With_Null_Returns_Zero()
		{
			var deck = new ArchetypeDeck();
			Assert.AreEqual(0, deck.Similarity(null));
		}

		[Test]
		public void Similarity_Of_NonEmpty_With_Empty_Is_One()
		{
			var deck = new ArchetypeDeck();
			Assert.AreEqual(1, deck.Similarity(new ArchetypeDeck()));
		}

		[Test]
		public void Similarity_Of_NonEmpty_With_Empty_Is_Zero()
		{
			var deck = new ArchetypeDeck();
			deck.Cards = new List<Card>() { new Card("A", "card", 1, null) };
			Assert.AreEqual(0, deck.Similarity(new ArchetypeDeck()));
		}
	}
}