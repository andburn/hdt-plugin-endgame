using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HDT.Plugins.EndGame.Tests
{
	[TestClass]
	public class NamingPatternTest
	{
		private NamingPattern np;

		[TestInitialize]
		public void Setup()
		{
			np = null;
		}

		//[TestMethod]
		//public void TestUnknownToken()
		//{
		//	Assert.IsFalse(NamingPattern.TryParse("{PlayerRank}", out np));
		//}

		//[TestMethod]
		//public void TestKnownToken()
		//{
		//	Assert.IsTrue(NamingPattern.TryParse("{PlayerName}", out np));
		//}

		[TestMethod]
		public void TestValidSingleTokenPattern()
		{
			Assert.IsTrue(NamingPattern.TryParse("{PlayerName}", out np));
		}

		[TestMethod]
		public void TestValidMultiTokenPattern()
		{
			Assert.IsTrue(NamingPattern.TryParse("Prefix {PlayerName} vs {OpponentName} Postfix", out np));
		}

		[TestMethod]
		public void TestInValidTokenPattern()
		{
			Assert.IsFalse(NamingPattern.TryParse("{nolower}", out np));
		}

		[TestMethod]
		public void TestPatternWithNoTokens()
		{
			Assert.IsTrue(NamingPattern.TryParse("Just a String", out np));
		}
	}
}
