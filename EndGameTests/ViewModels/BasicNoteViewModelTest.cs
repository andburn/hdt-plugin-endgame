using System;
using System.Collections.ObjectModel;
using HDT.Plugins.Common.Services;
using HDT.Plugins.EndGame.Models;
using HDT.Plugins.EndGame.Services;
using HDT.Plugins.EndGame.ViewModels;
using Moq;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class BasicNoteViewModelTest
	{
		private BasicNoteViewModel viewModel;
		private Mock<IDataRepository> trackMock;
		private Mock<ILoggingService> logMock;
		private Mock<IImageCaptureService> capMock;

		[SetUp]
		public void TestSetup()
		{
			logMock = new Mock<ILoggingService>();
			capMock = new Mock<IImageCaptureService>();
			trackMock = new Mock<IDataRepository>();

			trackMock.Setup(x => x.GetGameNote()).Returns("Note Text");

			viewModel = new BasicNoteViewModel(trackMock.Object, logMock.Object, capMock.Object);
		}

		[Test]
		public void DefaultNoteValue_ComesFromRepository()
		{
			Assert.That(viewModel.Note, Is.EqualTo("Note Text"));
		}

		[Test]
		public void NoteChange_UpdatesRepository()
		{
			viewModel.Note = "Changed Text";

			Assert.That(() =>
				trackMock.Verify(x => x.UpdateGameNote("Changed Text")),
				Throws.Nothing);
		}

		[Test]
		public void IfScreenshotsIsNullOnClosing_LogMessage()
		{
			viewModel.Screenshots = null;
			viewModel.WindowClosingCommand.Execute(null);

			Assert.That(() =>
				logMock.Verify(x => x.Debug("No screenshot selected (len=)")),
				Throws.Nothing);
		}

		[Test]
		public void OnClosing_SaveSelectedScreenshot()
		{
			viewModel.Screenshots = new ObservableCollection<Screenshot>() {
				new Screenshot(null, null, 0) { IsSelected = true }
			};
			viewModel.WindowClosingCommand.Execute(null);

			Assert.That(() =>
				capMock.Verify(x => x.SaveImage(It.IsAny<Screenshot>()), Times.Once),
				Throws.Nothing);
		}

		[Test]
		public void OnClosing_CatchExceptionsOnSavingImage()
		{
			capMock.Setup(x => x.SaveImage(It.IsAny<Screenshot>())).Throws<Exception>();
			viewModel.Screenshots = new ObservableCollection<Screenshot>() {
				new Screenshot(null, null, 0) { IsSelected = true }
			};
			viewModel.WindowClosingCommand.Execute(null);

			Assert.That(() =>
				logMock.Verify(x => x.Error(It.IsAny<string>()), Times.Once),
				Throws.Nothing);
		}
	}
}