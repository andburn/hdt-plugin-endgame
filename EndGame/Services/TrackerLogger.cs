using Hearthstone_Deck_Tracker.Utility.Logging;

namespace HDT.Plugins.EndGame.Services
{
	public class TrackerLogger : ILoggingService
	{
		public void Debug(object obj)
		{
			Log.Debug(obj.ToString());
		}

		public void Debug(string message)
		{
			Log.Debug(message);
		}

		public void Error(object obj)
		{
			Log.Error(obj.ToString());
		}

		public void Error(string message)
		{
			Log.Error(message);
		}

		public void Info(object obj)
		{
			Log.Info(obj.ToString());
		}

		public void Info(string message)
		{
			Log.Info(message);
		}
	}
}