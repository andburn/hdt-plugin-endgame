using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests
{
	[TestClass]
	public class UtilsTest
	{
		[TestMethod]
		public void KlassFromStringLowercase()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, Utils.KlassFromString("warrior"));
		}

		[TestMethod]
		public void KlassFromStringUppercase()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, Utils.KlassFromString("WARRIOR"));
		}

		[TestMethod]
		public void KlassFromStringWhiteSpace()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, Utils.KlassFromString(" warrIOR "));
		}

		[TestMethod]
		public void KlassFromStringUnknown()
		{
			Assert.AreEqual(PlayerClass.ANY, Utils.KlassFromString("Some String"));
		}

		[TestMethod]
		public void KlassFromStringNull()
		{
			Assert.AreEqual(PlayerClass.ANY, Utils.KlassFromString(null));
		}

		[TestMethod]
		public void ConvertFormatStandard()
		{
			Assert.AreEqual(GameFormat.STANDARD, Utils.ConvertFormat(Format.Standard));
		}

		[TestMethod]
		public void ConvertFormatWild()
		{
			Assert.AreEqual(GameFormat.WILD, Utils.ConvertFormat(Format.Wild));
		}

		[TestMethod]
		public void ConvertFormatNull()
		{
			Assert.AreEqual(GameFormat.ANY, Utils.ConvertFormat(null));
		}
	}
}