using HDT.Plugins.Common.Enums;
using HDT.Plugins.EndGame.Models;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class ArchetypeRecordTest
	{
		private ArchetypeRecord record;

		[SetUp]
		public void Setup()
		{
			record = new ArchetypeRecord();
		}

		[Test]
		public void DefaultCtor_HasDefaultValues()
		{
			Assert.That(record.Name, Is.EqualTo(ArchetypeRecord.DefaultName));
			Assert.That(record.Klass, Is.EqualTo(PlayerClass.ALL));
			Assert.That(record.TotalWins, Is.EqualTo(0));
			Assert.That(record.TotalLosses, Is.EqualTo(0));
		}

		[Test]
		public void ParamCtor_TakesNameAndClass()
		{
			var r = new ArchetypeRecord("Control", PlayerClass.MAGE);
			Assert.That(r.Name, Is.EqualTo("Control"));
			Assert.That(r.Klass, Is.EqualTo(PlayerClass.MAGE));
		}

		[Test]
		public void WinRate_IsZero_WithZeroGames()
		{
			Assert.That(record.WinRate, Is.EqualTo(0));
		}

		[Test]
		public void WinRate_IsZero_WithZeroWinsAndAnyLosses()
		{
			record.TotalLosses = 4;
			Assert.That(record.WinRate, Is.EqualTo(0));
		}

		[Test]
		public void WinRate_IsCorrect_ForAnyWinsAndNoLosses()
		{
			record.TotalWins = 1;
			Assert.That(record.WinRate, Is.EqualTo(100.0f));
		}

		[Test]
		public void WinRate_IsCorrect_ForAnyWinsAndAnyLosses()
		{
			record.TotalWins = 6;
			record.TotalLosses = 4;
			Assert.That(record.WinRate, Is.EqualTo(60.0f));
		}

		[Test]
		public void WinRateText_IsCorrect_ForAnyWinsAndAnyLosses()
		{
			record.TotalWins = 3;
			record.TotalLosses = 6;
			Assert.That(record.WinRateText, Is.EqualTo("33%"));
		}
	}
}