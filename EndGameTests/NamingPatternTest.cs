using System;
using System.Collections.Generic;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests
{
	[TestClass]
	public class NamingPatternTest
	{
		private NamingPattern np;
		private GameStats game;

		[TestInitialize]
		public void Setup()
		{
			np = null;
			game = new GameStats(GameResult.Loss, "Warlock", "Mage");
			game.PlayerName = "Player1";
			game.OpponentName = "Player2";
		}

		[TestMethod]
		public void SingleTokenPattern()
		{
			Assert.IsTrue(NamingPattern.TryParse("{PlayerName}", out np));
		}

		[TestMethod]
		public void MultiTokenPattern()
		{
			Assert.IsTrue(NamingPattern.TryParse("{PlayerName}{PlayerClass}", out np));
		}

		[TestMethod]
		public void MultiTokenPlusStringsPattern()
		{
			Assert.IsTrue(NamingPattern.TryParse("Prefix {PlayerName} vs {OpponentName} Postfix", out np));
		}

		[TestMethod]
		public void PatternWithNoTokens()
		{
			Assert.IsTrue(NamingPattern.TryParse("Just a String", out np));
		}

		[TestMethod]
		public void EmptyPatternReturnsDefault()
		{
			Assert.IsTrue(NamingPattern.TryParse("", out np));
			Assert.AreEqual("Player1 (Mage) VS Player2 (Warlock) " 
				+ DateTime.Now.ToString("dd.MM.yyy_HH.mm"), np.Apply(game));
		}

		[TestMethod]
		public void InValidTokens()
		{
			Assert.IsFalse(NamingPattern.TryParse("{nolower}", out np));
		}

		[TestMethod]
		public void ValidTokens()
		{
			Assert.IsTrue(NamingPattern.TryParse("{PlayerName}", out np));
		}

		[TestMethod]
		public void ParseTokensInCorrectOrder()
		{
			NamingPattern.TryParse("{PlayerName}{PlayerClass}", out np);
			Assert.AreEqual(2, np.Pattern.Count);
			Assert.AreEqual("{PlayerName}", np.Pattern[0]);
			Assert.AreEqual("{PlayerClass}", np.Pattern[1]);
		}

		[TestMethod]
		public void ParseFullPatternInCorrectOrder()
		{
			NamingPattern.TryParse(" Before {PlayerName} Middle {PlayerClass} After ", out np);
			Assert.AreEqual(5, np.Pattern.Count);
			Assert.AreEqual(" Before ", np.Pattern[0]);
			Assert.AreEqual("{PlayerName}", np.Pattern[1]);
			Assert.AreEqual(" Middle ", np.Pattern[2]);
			Assert.AreEqual("{PlayerClass}", np.Pattern[3]);
			Assert.AreEqual(" After ", np.Pattern[4]);
		}

		[TestMethod]
		public void PatternEvaluatesCorrectly()
		{
			NamingPattern.TryParse(" Before {PlayerName} ({PlayerClass}) VS {OpponentName} ({OpponentClass}) After ", out np);
			Assert.AreEqual(" Before Player1 (Mage) VS Player2 (Warlock) After ", np.Apply(game));
		}

		[TestMethod]
		public void ValidDatePattern()
		{
			NamingPattern.TryParse("{Date:ddMMyyy HH:mm}", out np);
			Assert.AreEqual(DateTime.Now.ToString("ddMMyyy HH:mm"), np.Apply(game));
		}

		[TestMethod]
		public void InValidDatePattern()
		{
			NamingPattern.TryParse("{Date:X}", out np);
			Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyy_HH.mm"), np.Apply(game));
		}

		[TestMethod]
		public void UnknownCustomDatePattern()
		{
			NamingPattern.TryParse("{Date:ABC}", out np);
			Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyy_HH.mm"), np.Apply(game));
		}		
	}
}
