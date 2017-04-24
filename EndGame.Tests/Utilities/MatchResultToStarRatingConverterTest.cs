using HDT.Plugins.Common.Models;
using NUnit.Framework;
using System.Windows.Data;
using HDT.Plugins.EndGame.Utilities;
using System.Globalization;
using System.Windows;
using HDT.Plugins.Common.Utils;
using HDT.Plugins.EndGame.Models;

namespace HDT.Plugins.EndGame.Tests.Models
{
	[TestFixture]
	public class MatchResultToStarRatingConverterTest
	{
		private IValueConverter converter;
		private CultureInfo culture;
		private static readonly string zero = IcoMoon.StarEmpty + IcoMoon.StarEmpty + IcoMoon.StarEmpty;
		private static readonly string one = IcoMoon.StarFull + IcoMoon.StarEmpty + IcoMoon.StarEmpty;
		private static readonly string two = IcoMoon.StarFull + IcoMoon.StarFull + IcoMoon.StarEmpty;
		private static readonly string three = IcoMoon.StarFull + IcoMoon.StarFull + IcoMoon.StarFull;

		[OneTimeSetUp]
		public void Init()
		{
			converter = new MatchResultToStarRatingConverter();
			culture = CultureInfo.CurrentCulture;
		}

		[Test]
		public void ConvertBack_IsNotSupported()
		{
			var result = converter.ConvertBack(0, typeof(int), 0, culture);
			Assert.AreEqual(DependencyProperty.UnsetValue, result);
		}

		[Test]
		public void Convert_IntsToZero()
		{
			Assert.AreEqual(zero, converter.Convert(1, null, null, culture));
		}

		[Test]
		public void Convert_StringsToZero()
		{
			Assert.AreEqual(zero, converter.Convert("0", null, null, culture));
		}

		[Test]
		public void Convert_ZeroToZeroStars()
		{
			Assert.AreEqual(zero, converter.Convert(0f, null, null, culture));
		}

		[Test]
		public void Convert_NegativeToZeroStars()
		{
			Assert.AreEqual(zero, converter.Convert(-1f, null, null, culture));
		}

		[Test]
		public void Convert_OneToThreeStars()
		{
			Assert.AreEqual(three, converter.Convert(1f, null, null, culture));
		}

		[Test]
		public void Convert_PointThreeToThreeStars()
		{
			Assert.AreEqual(three, converter.Convert(0.3f, null, null, culture));
		}

		[Test]
		public void Convert_PointFiveToThreeStars()
		{
			Assert.AreEqual(three, converter.Convert(0.5f, null, null, culture));
		}

		[Test]
		public void Convert_ThresholdToTwo()
		{
			Assert.AreEqual(two, converter.Convert(MatchResult.THRESHOLD, null, null, culture));
		}

		[Test]
		public void Convert_LessThresholdToOne()
		{
			Assert.AreEqual(one, converter.Convert(MatchResult.THRESHOLD - 0.01f, null, null, culture));
		}

		[Test]
		public void Convert_GreaterThresholdToTwo()
		{
			Assert.AreEqual(two, converter.Convert(MatchResult.THRESHOLD + 0.01f, null, null, culture));
		}
	}
}