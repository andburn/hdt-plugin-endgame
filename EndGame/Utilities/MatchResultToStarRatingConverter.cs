using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HDT.Plugins.Common.Utils;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Utilities
{
	public class MatchResultToStarRatingConverter : IValueConverter
	{
		private static readonly string zero = IcoMoon.StarEmpty + IcoMoon.StarEmpty + IcoMoon.StarEmpty;
		private static readonly string one = IcoMoon.StarFull + IcoMoon.StarEmpty + IcoMoon.StarEmpty;
		private static readonly string two = IcoMoon.StarFull + IcoMoon.StarFull + IcoMoon.StarEmpty;
		private static readonly string three = IcoMoon.StarFull + IcoMoon.StarFull + IcoMoon.StarFull;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var rating = zero;
			if (value is float)
			{
				var num = (float)value;
				if (num >= 0.03 && num < MatchResult.THRESHOLD)
					rating = one;
				else if (num >= MatchResult.THRESHOLD && num < 0.3)
					rating = two;
				else if (num >= 0.3)
					rating = three;
			}
			return rating;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}