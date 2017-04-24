using HDT.Plugins.EndGame.ViewModels;
using NUnit.Framework;

namespace HDT.Plugins.EndGame.Tests.ViewModels
{
	[TestFixture]
	public class EmptyNoteViewModelTest
	{
		[Test]
		public void DefaultNoteMessage()
		{
			var vm = new EmptyNoteViewModel();
			Assert.AreEqual("Not Available", vm.Message);
		}
	}
}