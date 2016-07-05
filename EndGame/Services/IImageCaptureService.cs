using System.Collections.ObjectModel;
using System.Threading.Tasks;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Services
{
	public interface IImageCaptureService
	{
		Task<ObservableCollection<Screenshot>> CaptureSequence(int delay, string dir, int num, int delayBetween);
	}
}