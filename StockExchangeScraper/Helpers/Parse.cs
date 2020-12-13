using System.Globalization;

namespace StockExchangeScraper
{
	static class Parse
	{
		public static double? Double(string toParse, string decimalSeparator, string groupSeparator)
		{
			if (toParse == "-") return null;

			return double.Parse(toParse, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, new NumberFormatInfo { NumberDecimalSeparator = decimalSeparator, NumberGroupSeparator = groupSeparator });
		}

		public static int? Int(string toParse)
		{
			if (toParse == "-") return null;

			return int.Parse(toParse, NumberStyles.AllowThousands, new NumberFormatInfo { NumberGroupSeparator = " " });
		}
	}
}
