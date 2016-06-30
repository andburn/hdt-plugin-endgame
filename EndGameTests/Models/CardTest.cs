using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestClass]
	public class CardTest
	{
		private static Card _cardA;
		private static Card _cardB;
		private static Card _cardC;
		private static Card _zero;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_cardA = new Card("NT_001", "card", 1, null);
			_cardB = new Card("NT_001", "card", 3, null);
			_cardC = new Card("NT_051", "card", 2, null);
			_zero = new Card("NT_001", "card", 0, null);
		}

		[TestMethod]
		public void NotEqualToNull()
		{
			Assert.AreNotEqual(_cardA, null);
		}

		[TestMethod]
		public void EqualToSelf()
		{
			Assert.AreEqual(_cardA, _cardA);
			Assert.AreSame(_cardA, _cardA);
		}

		[TestMethod]
		public void EqualIfIdIsSame()
		{
			Assert.AreEqual(_cardA, _cardB);
			Assert.AreNotSame(_cardA, _cardB);
			Assert.AreEqual(_cardA.GetHashCode(), _cardB.GetHashCode());
		}
	}
}