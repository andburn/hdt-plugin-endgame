using System.Threading.Tasks;
using HDT.Plugins.EndGame.ViewModels;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class MainViewModelTest
	{
		private MainViewModel viewModel;

		[SetUp]
		public void TestSetup()
		{
			viewModel = new MainViewModel();
		}

		[Test]
		public async Task NavigationTest()
		{
			viewModel.ContentViewModel = null;
			viewModel.ContentTitle = null;
			await viewModel.OnNavigation("RanDom");
			Assert.That(viewModel.ContentViewModel, Is.Null);
			Assert.That(viewModel.ContentTitle, Is.Null);
			await viewModel.OnNavigation("SeTTings");
			Assert.That(viewModel.ContentViewModel, Is.InstanceOf<SettingsViewModel>());
			Assert.That(viewModel.ContentTitle, Is.EqualTo("Settings"));
		}
	}
}