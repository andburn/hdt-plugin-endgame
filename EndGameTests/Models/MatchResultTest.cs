using HDT.Plugins.EndGame.Models;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class MatchResultTest
	{
		[Test]
		public void CompareTo_Self_IsZero()
		{
			var result = new MatchResult(null, 0.4f, 0.6f);
			Assert.AreEqual(0, result.CompareTo(result));
		}

		[Test]
		public void CompareTo_By_Similarity()
		{
			var result = new MatchResult(null, 0.4f, 0.6f);
			Assert.AreEqual(1, result.CompareTo(
				new MatchResult(null, 0.3f, 0.6f)));
		}

		[Test]
		public void CompareTo_By_Similarity_Then_Containment()
		{
			var result = new MatchResult(null, 0.4f, 0.6f);
			Assert.AreEqual(-1, result.CompareTo(
				new MatchResult(null, 0.4f, 0.8f)));
		}
	}
}