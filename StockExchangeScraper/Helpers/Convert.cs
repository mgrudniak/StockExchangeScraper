//using StockExchangeScraper.Models;
//using System.Collections.Generic;

//namespace StockExchangeScraper
//{
//	static class Convert
//	{
//		public static DataLibrary.Models.Country ToDataLibraryCountry(string country)
//		{
//			return new DataLibrary.Models.Country { Name = country };
//		}

//		public static DataLibrary.Models.Company ToDataLibraryCompany(CompanyModel company)
//		{
//			return new DataLibrary.Models.Company
//			{
//				Isin = company.Isin,
//				Name = company.Name,
//				Sector = new DataLibrary.Models.Sector { Name = company.Sector },
//				Industry = new DataLibrary.Models.Industry { Name = company.Industry },
//				EquityType = new DataLibrary.Models.EquityType { Name = company.EquityType },
//				Exchange = new DataLibrary.Models.Exchange { Name = company.Exchange },
//				EmployeesNumber = company.EmployeesNumber,
//				Url = company.Url
//			};
//		}

//		public static List<DataLibrary.Models.ExchangeHoliday> ToDataLibraryExchangeHoliday(List<ExchangeHolidayModel> holidays, string exchange)
//		{
//			List<DataLibrary.Models.ExchangeHoliday> newHolidays = new List<DataLibrary.Models.ExchangeHoliday>();
//			foreach (var holiday in holidays)
//			{
//				newHolidays.Add(new DataLibrary.Models.ExchangeHoliday
//				{
//					Exchange = new DataLibrary.Models.Exchange { Name = exchange },
//					Date = holiday.Date
//				});
//			}
//			return newHolidays;
//		}

//		public static List<DataLibrary.Models.StockQuote> ToDataLibraryStockQuote(List<StockQuoteModel> quotes)
//		{
//			List<DataLibrary.Models.StockQuote> newQuotes = new List<DataLibrary.Models.StockQuote>();
//			foreach (var quote in quotes)
//			{
//				newQuotes.Add(new DataLibrary.Models.StockQuote
//				{
//					CompanyIsin = quote.CompanyIsin,
//					Date = quote.Date,
//					Open = quote.Open,
//					Close = quote.Close,
//					Min = quote.Min,
//					Max = quote.Max,
//					Volume = quote.Volume
//				});
//			}
//			return newQuotes;
//		}
//	}
//}
