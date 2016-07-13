using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Utilities
{
	public class MatchResultToStarRatingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var rating = "\ue9d7\ue9d7\ue9d7";
			if (value is float)
			{
				var num = (float)value;
				if (num >= 0.03 && num < MatchResult.THRESHOLD)
					rating = "\ue9d9\ue9d7\ue9d7";
				else if (num >= MatchResult.THRESHOLD && num < 0.3)
					rating = "\ue9d9\ue9d9\ue9d7";
				else if (num >= 0.3)
					rating = "\ue9d9\ue9d9\ue9d9";
			}
			return rating;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}