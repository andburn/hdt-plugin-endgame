using HDT.Plugins.EndGame.Models;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class CardTest
	{
		private Card _cardA;
		private Card _cardB;
		private Card _cardC;
		private Card _zero;

		[OneTimeSetUp]
		public void Init()
		{
			_cardA = new Card("NT_001", "card", 1, null);
			_cardB = new Card("NT_001", "card", 3, null);
			_cardC = new Card("NT_051", "card", 2, null);
			_zero = new Card("NT_001", "card", 0, null);
		}

		[Test]
		public void NotEqualToNull()
		{
			Assert.AreNotEqual(_cardA, null);
		}

		[Test]
		public void EqualToSelf()
		{
			Assert.AreEqual(_cardA, _cardA);
			Assert.AreSame(_cardA, _cardA);
		}

		[Test]
		public void EqualIfIdIsSame()
		{
			Assert.AreEqual(_cardA, _cardB);
			Assert.AreNotSame(_cardA, _cardB);
			Assert.AreEqual(_cardA.GetHashCode(), _cardB.GetHashCode());
		}
	}
}