using System.Collections.Generic;
using HDT.Plugins.EndGame.Enums;
using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestClass]
	public class ArchetypeDeckTest
	{
		[TestMethod]
		public void DefaultConstructorHasDefaultProps()
		{
			var deck = new ArchetypeDeck();
			Assert.IsNull(deck.Name);
			Assert.IsNull(deck.Style);
			Assert.AreEqual(PlayerClass.ANY, deck.Klass);
			Assert.AreEqual(GameFormat.ANY, deck.Format);
			Assert.IsTrue(deck.Cards.Count == 0);
		}

		[TestMethod]
		public void ParamConstructorAssignsProps()
		{
			var deck = new ArchetypeDeck(
				"Control Warrior",
				PlayerClass.WARRIOR,
				GameFormat.STANDARD,
				ArchetypeStyles.CONTROL,
				new List<Card>()
			);
			Assert.AreEqual("Control Warrior", deck.Name);
			Assert.AreEqual(ArchetypeStyles.CONTROL, deck.Style);
			Assert.AreEqual(PlayerClass.WARRIOR, deck.Klass);
			Assert.AreEqual(GameFormat.STANDARD, deck.Format);
			Assert.IsTrue(deck.Cards.Count == 0);
		}

		[TestMethod]
		public void TestToString()
		{
			var deck = new ArchetypeDeck(
				"Control Warrior",
				PlayerClass.WARRIOR,
				GameFormat.STANDARD,
				ArchetypeStyles.CONTROL,
				new List<Card>()
			);
			Assert.AreEqual("Control Warrior", deck.ToString());
		}

		[TestMethod]
		public void TestToNoteString()
		{
			var deck = new ArchetypeDeck(
				"Control Warrior",
				PlayerClass.WARRIOR,
				GameFormat.STANDARD,
				ArchetypeStyles.CONTROL,
				new List<Card>()
			);
			Assert.AreEqual("Control Warrior : WARRIOR.CONTROL", deck.ToNoteString());
		}
	}
}