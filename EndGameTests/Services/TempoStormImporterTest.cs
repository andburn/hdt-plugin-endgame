using System;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.Services.TempoStorm;
using Moq;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.Services
{
	[TestFixture]
	public class TempoStormImporterTest
	{
		private Mock<IDataRepository> _data;
		private Mock<IHttpClient> _http;
		private Mock<ILoggingService> _log;

		[OneTimeSetUp]
		public void SetUp()
		{
			_data = new Mock<IDataRepository>();
			_http = new Mock<IHttpClient>();
			_log = new Mock<ILoggingService>();
		}

		[Test]
		public void GetSnapshotSlug()
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(x => x.JsonGet(It.IsAny<string>()))
				.ReturnsAsync(@"{""snapshotType"":""standard"",""slugs"":[{""slug"":""2016-07-10""}]}");

			var importer = new SnapshotImporter(mock.Object, _data.Object, _log.Object);

			Assert.That(async () => await importer.GetSnapshotSlug("standard"),
				Is.EqualTo(new Tuple<string, string>("2016-07-10", "standard")));
		}

		[Test]
		public void GetSnapshotSlug_Error()
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(x => x.JsonGet(It.IsAny<string>()))
				.ReturnsAsync(@"{""error"":{""status"":500}}");

			var importer = new SnapshotImporter(mock.Object, _data.Object, _log.Object);

			Assert.That(async () => await importer.GetSnapshotSlug("standard"),
				Throws.TypeOf<ImportException>()
				.With.Message.EqualTo("Getting the snapshot slug failed (500)"));
		}

		[Test]
		public void GetSnapshotSlug_Unexpected()
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(x => x.JsonGet(It.IsAny<string>()))
				.ReturnsAsync(@"{""snapshotType"":""standard"",""slugs"":[{""slug"":""2016-07-10""},{""slug"":""2016-06-10""}]}");

			var importer = new SnapshotImporter(mock.Object, _data.Object, _log.Object);

			Assert.That(async () => await importer.GetSnapshotSlug("standard"),
				Throws.TypeOf<ImportException>()
				.With.Message.EqualTo("Snapshot slug count greater than one"));
		}

		[Test]
		public void GetSnapshot()
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(x => x.JsonGet(It.IsAny<string>()))
				.ReturnsAsync(@"{""title"":""Salt for all""}");
			var importer = new SnapshotImporter(mock.Object, _data.Object, _log.Object);

			var result = importer.GetSnapshot(new Tuple<string, string>("standard", "2016-09-10")).Result;

			Assert.That(result.Title, Is.EqualTo("Salt for all"));
		}

		[Test]
		public void GetSnapshot_Error()
		{
			var mock = new Mock<IHttpClient>();
			mock.Setup(x => x.JsonGet(It.IsAny<string>()))
				.ReturnsAsync(@"{""error"":{""status"":500}}");
			var importer = new SnapshotImporter(mock.Object, _data.Object, _log.Object);

			Assert.That(async () => await importer.GetSnapshot(new Tuple<string, string>("standard", "2016-09-10")),
				Throws.TypeOf<ImportException>()
				.With.Message.EqualTo("Getting the snapshot failed (500)"));
		}
	}
}