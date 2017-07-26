using HDT.Plugins.EndGame.Services;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Services
{
	[TestFixture]
	public class UpdateResultTest
	{
		[Test]
		public void HasUpdates_WhenStandardIsNewer()
		{
			var update = new UpdateResult("2016-05-04", "2016-06-08", "2016-05-05", "2016-05-05");
			Assert.That(update.HasUpdates(), Is.True);
		}

		[Test]
		public void HasUpdates_WhenWildIsNewer()
		{
			var update = new UpdateResult("2016-05-05", "2016-05-05", "2016-05-04", "2016-06-08");
			Assert.That(update.HasUpdates(), Is.True);
		}

		[Test]
		public void HasUpdates_WhenBothAreNewer()
		{
			var update = new UpdateResult("2016-05-04", "2016-06-08", "2016-05-04", "2016-06-08");
			Assert.That(update.HasUpdates(), Is.True);
		}

		[Test]
		public void HasNoUpdates_WhenWildIsNewerButExcluded()
		{
			var update = new UpdateResult("2016-05-05", "2016-05-05", "2016-05-04", "2016-06-08");
			Assert.That(update.HasUpdates(false), Is.False);
		}

		[Test]
		public void HasNoUpdates_WhenLatestIsNullOrEmtpy()
		{
			var update = new UpdateResult("2016-05-05", null, "2016-06-08", null);
			Assert.That(update.HasUpdates(), Is.False);
		}

		[Test]
		public void HasUpdates_WhenPreviousIsNullOrEmtpy()
		{
			var update = new UpdateResult(null, "2016-05-05", null, "2016-06-08");
			Assert.That(update.HasUpdates(), Is.True);
		}
	}
}