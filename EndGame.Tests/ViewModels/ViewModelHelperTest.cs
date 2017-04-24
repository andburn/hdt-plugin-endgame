using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.ViewModels;
using Moq;
using NUnit.Framework;
using static HDT.Plugins.EndGame.ViewModels.ViewModelHelper;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class ViewModelHelperTest
	{
		private List<ArchetypeDeck> _archetypes;

		[OneTimeSetUp]
		public void Init()
		{
			_archetypes = new List<ArchetypeDeck>() {
				TestHelper.CreateDeck("one", PlayerClass.DRUID, true, "DR_123:1;DR_456:1;NT_001:1"),
				TestHelper.CreateDeck("two", PlayerClass.MAGE, true, "MG_123:1;NT_001:1;MG_789:1"),
				TestHelper.CreateDeck("three", PlayerClass.MAGE, true, "MG_123:1;MG_456:1;MG_789:1"),
				TestHelper.CreateDeck("four", PlayerClass.MAGE, false, "MG_123:1;NT_001:1;MG_789:1")
			};
		}

		[Test]
		public void MatchSameClassStandardSingle()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.DRUID, true, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[Test]
		public void MatchSameClassWildSingle()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.DRUID, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[Test]
		public void MatchSameClassStandardMultiple()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.MAGE, true, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("two", results.First().Deck.Name);
		}

		[Test]
		public void MatchSameClassWildMultiple()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.MAGE, false, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.67f, results.First().Similarity);
		}

		[Test]
		public void MatchAnyClassStandard()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.ALL, true, "NT_001:2");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.25f, results.First().Similarity);
		}

		[Test]
		public void SortedBySimilarity()
		{
			var deck = TestHelper.CreateDeck(PlayerClass.ALL, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(4, results.Count);
			Assert.AreEqual("one", results[0].Deck.Name);
			Assert.AreEqual("four", results[1].Deck.Name);
			Assert.AreEqual("two", results[2].Deck.Name);
			Assert.AreEqual("three", results[3].Deck.Name);
		}
	}
}