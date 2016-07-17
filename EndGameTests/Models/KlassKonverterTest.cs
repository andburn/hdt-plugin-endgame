using HDT.Plugins.EndGame.Models;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class KlassKonverterTest
	{
		[Test]
		public void NullOrEmptyString_Should_Be_Any()
		{
			Assert.AreEqual(Klass.Any, KlassKonverter.FromString(string.Empty));
			Assert.AreEqual(Klass.Any, KlassKonverter.FromString(null));
		}

		[Test]
		public void Druid_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Druid, KlassKonverter.FromString("Druid"));
		}

		[Test]
		public void Hunter_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Hunter, KlassKonverter.FromString("Hunter"));
		}

		[Test]
		public void Mage_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Mage, KlassKonverter.FromString("Mage"));
		}

		[Test]
		public void Paladin_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Paladin, KlassKonverter.FromString("Paladin"));
		}

		[Test]
		public void Priest_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Priest, KlassKonverter.FromString("Priest"));
		}

		[Test]
		public void Rogue_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Rogue, KlassKonverter.FromString("Rogue"));
		}

		[Test]
		public void Shaman_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Shaman, KlassKonverter.FromString("Shaman"));
		}

		[Test]
		public void Warlockn_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Warlock, KlassKonverter.FromString("Warlock"));
		}

		[Test]
		public void Warrior_Should_Be_Correct()
		{
			Assert.AreEqual(Klass.Warrior, KlassKonverter.FromString("Warrior"));
		}

		[Test]
		public void NonKlass_Should_Be_Any()
		{
			Assert.AreEqual(Klass.Any, KlassKonverter.FromString("Random String"));
		}
	}
}