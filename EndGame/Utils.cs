using System;
using HDT.Plugins.EndGame.Enums;
using Hearthstone_Deck_Tracker.Enums;

namespace HDT.Plugins.EndGame
{
	public static class Utils
	{
		public static PlayerClass KlassFromString(string klass)
		{
			PlayerClass result;
			var success = Enum.TryParse(klass, true, out result);
			if (success)
				return result;

			return PlayerClass.ANY;
		}

		public static GameFormat ConvertFormat(Format? format)
		{
			switch (format)
			{
				case Format.Standard:
					return GameFormat.STANDARD;

				case Format.Wild:
					return GameFormat.WILD;

				default:
					return GameFormat.ANY;
			}
		}
	}
}