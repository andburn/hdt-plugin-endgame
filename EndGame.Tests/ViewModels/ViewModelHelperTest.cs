using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.ViewModels;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using static HDT.Plugins.EndGame.Tests.TestHelper;
using static HDT.Plugins.EndGame.ViewModels.ViewModelHelper;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class ViewModelHelperTest
	{
		private List<ArchetypeDeck> archetypes;
		private List<Game> games;
		private List<Deck> decks;
		private Mock<IDataRepository> mock;

		[OneTimeSetUp]
		public void Setup()
		{
			archetypes = new List<ArchetypeDeck>() {
				CreateDeck("one", PlayerClass.DRUID, true, "DR_123:1;DR_456:1;NT_001:1"),
				CreateDeck("two", PlayerClass.MAGE, true, "MG_123:1;NT_001:1;MG_789:1"),
				CreateDeck("three", PlayerClass.MAGE, true, "MG_123:1;MG_456:1;MG_789:1"),
				CreateDeck("four", PlayerClass.MAGE, false, "MG_123:1;NT_001:1;MG_789:1")
			};
			decks = new List<Deck>() {
				CreateDeck(PlayerClass.DRUID, true, "IC_393:2"),
				CreateDeck(PlayerClass.MAGE, true, "XT_321:2"),
				CreateDeck(PlayerClass.HUNTER, false, "OG_322:1")
			};
			games = new List<Game>() {
				CreateGame(GameResult.WIN, PlayerClass.MAGE, PlayerClass.DRUID, decks[1], "one"),
				CreateGame(GameResult.LOSS, PlayerClass.MAGE, PlayerClass.MAGE, decks[1], "two"),
				CreateGame(GameResult.WIN, PlayerClass.DRUID, PlayerClass.MAGE, decks[0], null),
				CreateGame(GameResult.DRAW, PlayerClass.DRUID, PlayerClass.SHAMAN, decks[0], null),
				CreateGame(GameResult.WIN, PlayerClass.MAGE, PlayerClass.ROGUE, decks[1], "one")
			};
			mock = new Mock<IDataRepository>();
			mock.Setup(x => x.GetAllGames()).Returns(games);
			mock.Setup(x => x.GetAllDecks()).Returns(decks);
			mock.Setup(x => x.GetAllGamesWithDeck(decks[1].Id))
				.Returns(new List<Game>() { games[0], games[1], games[4] });
		}

		[Test]
		public void MatchSameClassStandardSingle()
		{
			var deck = CreateDeck(PlayerClass.DRUID, true, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(GameFormat.STANDARD, deck, archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[Test]
		public void MatchSameClassWildSingle()
		{
			var deck = CreateDeck(PlayerClass.DRUID, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(GameFormat.WILD, deck, archetypes);
			Assert.AreEqual("one", results.Single().Deck.Name);
		}

		[Test]
		public void MatchSameClassStandardMultiple()
		{
			var deck = CreateDeck(PlayerClass.MAGE, true, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(GameFormat.STANDARD, deck, archetypes);
			Assert.AreEqual(2, results.Count);
			Assert.AreEqual("two", results.First().Deck.Name);
		}

		[Test]
		public void MatchSameClassWildMultiple()
		{
			var deck = CreateDeck(PlayerClass.MAGE, false, "MG_123:1;NT_001:1");
			var results = MatchArchetypes(GameFormat.WILD, deck, archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.67f, results.First().Similarity);
		}

		[Test]
		public void MatchAnyClassStandard()
		{
			var deck = CreateDeck(PlayerClass.ALL, true, "NT_001:2");
			var results = MatchArchetypes(GameFormat.STANDARD, deck, archetypes);
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0.25f, results.First().Similarity);
		}

		[Test]
		public void SortedBySimilarity()
		{
			var deck = CreateDeck(PlayerClass.ALL, false, "DR_123:1;NT_001:1");
			var results = MatchArchetypes(GameFormat.WILD, deck, archetypes);
			Assert.AreEqual(4, results.Count);
			Assert.AreEqual("one", results[0].Deck.Name);
			Assert.AreEqual("four", results[1].Deck.Name);
			Assert.AreEqual("two", results[2].Deck.Name);
			Assert.AreEqual("three", results[3].Deck.Name);
		}

		[Test]
		public void GetDecksWithArchetypeGames()
		{
			var decks = ViewModelHelper.GetDecksWithArchetypeGames(mock.Object);
			Assert.That(decks.Count, Is.EqualTo(1));
		}

		[Test]
		public void GetDecksArchetypeStats()
		{
			var games = mock.Object.GetAllGamesWithDeck(decks[1].Id);
			var stats = GetArchetypeStats(games);
			Assert.That(stats.Count, Is.EqualTo(3));
		}
	}
}