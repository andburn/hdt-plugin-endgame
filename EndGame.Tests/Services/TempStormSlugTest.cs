using HDT.Plugins.EndGame.Services.TempoStorm;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Services
{
	[TestFixture]
	public class TempStormSlugTest
	{
		[Test]
		public void DefaultCtor_InitsEmptyItemsList()
		{
			var slug = new Slugs();
			Assert.NotNull(slug.Items);
			Assert.AreEqual(0, slug.Items.Count);
		}
	}
}