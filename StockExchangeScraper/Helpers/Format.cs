using System.Text.RegularExpressions;

namespace StockExchangeScraper
{
	static class Format
	{
		public static double? Volume(string volumeStr)
		{
			if (volumeStr == "-") return null;

			var match = Regex.Match(volumeStr, "[a-zA-Z]");
			string letter = "";
			if (match.Success)
			{
				letter = match.Value;

				volumeStr = volumeStr[0..^1];
			}

			int multiplier = 1;
			switch (letter)
			{
				case "K":
					multiplier = 1000;
					break;
				case "M":
					multiplier = 1000000;
					break;
				case "B":
					multiplier = 1000000000;
					break;
			}

			return double.Parse(volumeStr) * multiplier;
		}
	}
}
