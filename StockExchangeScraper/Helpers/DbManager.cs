using Microsoft.EntityFrameworkCore;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchangeScraper
{
	static class DbManager
	{
		public static void InsertCountryExchange(Country country, List<string> exchanges)
		{
			using (var context = new StockExchangeDbContext())
			{
				context.Countries.Add(country);
				context.SaveChanges();

				int countryId = country.CountryId;

				List<Exchange> newExchanges = new List<Exchange>();

				foreach (var exchange in exchanges)
				{
					newExchanges.Add(new Exchange { Name = exchange, CountryId = countryId });
				}

				context.Exchanges.AddRange(newExchanges);
				context.SaveChanges();
			}
		}

		public static void InsertCompanies(Company company)
		{
			using (var context = new StockExchangeDbContext())
			{
				context.Database.ExecuteSqlInterpolated(@$"EXEC dbo.InsertCompany @Isin={company.Isin}, @Name={company.Name}, @Sector={company.Sector.Name}, 
																				  @Industry={company.Industry.Name}, @EquityType={company.EquityType.Name}, 
																				  @Exchange={company.Exchange.Name}, @EmployeesNumber={company.EmployeesNumber},
																				  @Url={company.Url}");
			}
		}

		public static void InsertHolidays(List<ExchangeHoliday> holidays)
		{
			using (var context = new StockExchangeDbContext())
			{
				foreach (var holiday in holidays)
				{
					context.Database.ExecuteSqlInterpolated(@$"EXEC dbo.InsertExchangeHoliday @ExchangeName={holiday.Exchange.Name}, @Date={holiday.Date}");
				}
			}
		}

		public static void InsertStockQuote(List<StockQuote> stocks)
		{
			using (var context = new StockExchangeDbContext())
			{
				foreach (var stock in stocks)
				{
					context.StockQuotes.Add(stock);
				}
				context.SaveChanges();
			}
		}

		public static bool CompanyIsToDownload(string url)
		{
			bool isToDownload;
			using (var context = new StockExchangeDbContext())
			{
				isToDownload = context.Companies.Any(x => x.Url == url);
			}
			return !isToDownload;
		}

		public static bool StockIsToDownload(string url, string start, string end)
		{
			bool isToDownload;
			DateTime startDate = DateTime.Parse(start);
			DateTime endDate = DateTime.Parse(end);

			using (var context = new StockExchangeDbContext())
			{
				isToDownload = context.StockQuotes.Any(x => x.Company.Url == url &&
													   x.Date.CompareTo(startDate) >= 0 &&
													   x.Date.CompareTo(endDate) <= 0);
			}
			return !isToDownload;
		}

		public static bool IsHoliday()
		{
			bool isHoliday;
			using (var context = new StockExchangeDbContext())
			{
				isHoliday = context.ExchangeHolidays.Any(x => x.Date == DateTime.Now.Date);
			}
			return isHoliday;
		}
	}
}
