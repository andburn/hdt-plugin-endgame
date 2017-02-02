using HDT.Plugins.Common.Util;
using NUnit.Framework;
using static HDT.Plugins.Common.Util.EnumConverter;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class KlassKonverterTest
	{
		[Test]
		public void NullOrEmptyString_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass(string.Empty));
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass(null));
		}

		[Test]
		public void Druid_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.DRUID, ConvertHeroClass("Druid"));
		}

		[Test]
		public void Hunter_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.HUNTER, ConvertHeroClass("Hunter"));
		}

		[Test]
		public void Mage_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.MAGE, ConvertHeroClass("Mage"));
		}

		[Test]
		public void Paladin_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PALADIN, ConvertHeroClass("Paladin"));
		}

		[Test]
		public void Priest_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.PRIEST, ConvertHeroClass("Priest"));
		}

		[Test]
		public void Rogue_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.ROGUE, ConvertHeroClass("Rogue"));
		}

		[Test]
		public void Shaman_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.SHAMAN, ConvertHeroClass("Shaman"));
		}

		[Test]
		public void Warlockn_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARLOCK, ConvertHeroClass("Warlock"));
		}

		[Test]
		public void Warrior_Should_Be_Correct()
		{
			Assert.AreEqual(PlayerClass.WARRIOR, ConvertHeroClass("Warrior"));
		}

		[Test]
		public void NonKlass_Should_Be_Any()
		{
			Assert.AreEqual(PlayerClass.ALL, ConvertHeroClass("Random String"));
		}
	}
}