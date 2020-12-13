using System;

namespace StockExchangeScraper
{
	class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				if (DateTime.Now.Hour == 21)
				{
					try
					{
						//CountryExchangeScraper countryExchangeScraper = new CountryExchangeScraper();
						//countryExchangeScraper.DownloadCountriesExchanges();

						//CompaniesScraper companiesScraper = new CompaniesScraper();
						//companiesScraper.DownloadCompanies();

						//HistoricalQuotesScraper historicalQuotesScraper = new HistoricalQuotesScraper();
						//historicalQuotesScraper.DownloadHistorical(start: "11/30/2020", end: "12/01/2020");

						//HolidaysScraper holidaysScraper = new HolidaysScraper();
						//holidaysScraper.DownloadWarsawHolidays();

						QuotesScraper quotesScraper = new QuotesScraper();
						quotesScraper.DownloadWarsaw();

						if (DateTime.Now.Month == 12 && DateTime.Now.Day == 15)
						{
							HolidaysScraper holidaysScraper = new HolidaysScraper();
							holidaysScraper.DownloadWarsawHolidays();
						}
					}
					catch (InvalidTimeException e)
					{
						Console.WriteLine(e.Message);
						System.Threading.Thread.Sleep(TimeSpan.FromMinutes(30));
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						Console.WriteLine(e.StackTrace);
						Console.WriteLine(e.InnerException);
						Console.WriteLine(e.Data);
						Console.WriteLine(e.Source);
						Console.WriteLine(e.TargetSite);
					}
				}
				System.Threading.Thread.Sleep(TimeSpan.FromHours(1));
			}
		}
	}
}
