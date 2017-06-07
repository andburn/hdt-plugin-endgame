using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.ViewModels;
using HDT.Plugins.Common.Services;
using Moq;
using HDT.Plugins.EndGame.Utilities;
using HDT.Plugins.Common.Enums;
using HDT.Plugins.Common.Models;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class BasicNoteViewModelTest
	{
		private BasicNoteViewModel viewModel;
		private Mock<IDataRepository> trackMock;
		private Mock<ILoggingService> logMock;

		[SetUp]
		public void TestSetup()
		{
			var deck = new Deck(PlayerClass.DRUID, true);
			deck.Cards = new List<Card>();

			logMock = new Mock<ILoggingService>();
			trackMock = new Mock<IDataRepository>();

			trackMock.Setup(x => x.GetGameNote()).Returns("Note Text");
			trackMock.Setup(x => x.GetOpponentDeck()).Returns(deck);
			trackMock.Setup(x => x.GetAllDecksWithTag(Strings.ArchetypeTag)).Returns(new List<Deck>());

			viewModel = new BasicNoteViewModel(trackMock.Object, logMock.Object);
		}

		[Test]
		public async Task Properties_AreSetCorrectly_OnUpdate()
		{
			Assert.IsNull(viewModel.PlayerClass);
			Assert.IsFalse(viewModel.IsNoteFocused);
			await viewModel.Update();
			Assert.AreEqual("Note Text", viewModel.Note);
			Assert.AreEqual("DRUID", viewModel.PlayerClass);
			Assert.IsTrue(viewModel.IsNoteFocused);
		}
	}
}
