using System.Collections.Generic;
using System.Linq;
using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static HDT.Plugins.EndGame.ViewModels.ViewModelHelper;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestClass]
	public class ViewModelHelperTest
	{
		private static List<ArchetypeDeck> _archetypes;

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			_archetypes = new List<ArchetypeDeck>() {
				TestHelper.CreateDeck("one", Klass.Druid, true, "DR_123:1;DR_456:1;NT_001:1"),
				TestHelper.CreateDeck("two", Klass.Mage, true, "MG_123:1;NT_001:1;MG_789:1"),
				TestHelper.CreateDeck("three", Klass.Mage, true, "MG_123:1;MG_456:1;MG_789:1"),
				TestHelper.CreateDeck("four", Klass.Mage, false, "MG_123:1;NT_001:1;MG_789:1")
			};
		}

		[TestMethod]
		public void MatchSameClassStandardSingle()
		{
			var deck = TestHelper.CreateDeck(Klass.Druid, true, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[TestMethod]
		public void MatchSameClassWildSingle()
		{
			var deck = TestHelper.CreateDeck(Klass.Druid, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[TestMethod]
		public void MatchSameClassStandardMultiple()
		{
			var deck = TestHelper.CreateDeck(Klass.Mage, true, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("two", results.First().Deck.Name);
		}

		[TestMethod]
		public void MatchSameClassWildMultiple()
		{
			var deck = TestHelper.CreateDeck(Klass.Mage, false, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.67f, results.First().Similarity);
		}

		[TestMethod]
		public void MatchAnyClassStandard()
		{
			var deck = TestHelper.CreateDeck(Klass.Any, true, "NT_001:2");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.25f, results.First().Similarity);
		}

		[TestMethod]
		public void SortedBySimilarity()
		{
			var deck = TestHelper.CreateDeck(Klass.Any, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(deck, _archetypes);
			Assert.AreEqual(4, results.Count);
			Assert.AreEqual("one", results[0].Deck.Name);
			Assert.AreEqual("four", results[1].Deck.Name);
			Assert.AreEqual("two", results[2].Deck.Name);
			Assert.AreEqual("three", results[3].Deck.Name);
		}
	}
}