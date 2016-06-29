using HDT.Plugins.EndGame.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestClass]
	public class CardTest
	{
		private static Card _card;

		[ClassInitialize]
		public static void Setup(TestContext context)
		{
			_card = new Card("123", "ABC", 0, null);
		}

		[TestMethod]
		public void EqualityWithNull()
		{
			Assert.AreNotEqual(_card, null);
		}

		[TestMethod]
		public void EqualityWithSelf()
		{
			Assert.AreEqual(_card, _card);
			Assert.AreSame(_card, _card);
		}

		[TestMethod]
		public void EqualityWithSameId()
		{
			var card = new Card("123", "DEF", 1, null);
			Assert.AreEqual(_card, card);
			Assert.AreNotSame(_card, card);
			Assert.AreEqual(_card.GetHashCode(), card.GetHashCode());
		}
	}
}