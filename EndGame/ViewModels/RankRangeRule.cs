using System;
using System.Globalization;
using System.Windows.Controls;

namespace HDT.Plugins.EndGame.ViewModels
{
    public class RankRangeRule : ValidationRule
    {
        public int Max { get; set; } = 0;
        public int Min { get; set; } = 25;

        public RankRangeRule()
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int rank = 0;
            var success = int.TryParse(value as string, out rank);
            if (!success)
            {
                return new ValidationResult(false, "Rank must be a number.");
            }
            if (rank > Min || rank < Max)
            {
                return new ValidationResult(false,
                    $"Please enter a rank in the range: {Min}-{Max}");
            }
            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}