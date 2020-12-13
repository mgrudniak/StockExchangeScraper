using OpenQA.Selenium;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace StockExchangeScraper
{
	class HolidaysScraper : Scraper
	{
		public void DownloadWarsawHolidays()
		{
			string url = "https://www.gpw.pl/szczegoly-sesji";
			driver.Url = url;
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//ul[@id='tabs-0']/li[2]/a")).Click();
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));

			int year = DateTime.Now.Year + 1;
			List<ExchangeHoliday> holidays = new List<ExchangeHoliday>();

			var table = driver.FindElements(By.XPath($"//h2[text()='{year}']/following-sibling::table//tr/td[2]"));

			string exchangeName = "Warsaw";
			foreach (var row in table)
			{
				holidays.Add(new ExchangeHoliday
				{
					Exchange = new Exchange { Name = exchangeName },
					Date = DateTime.ParseExact(row.Text, "d MMMM", CultureInfo.GetCultureInfo("pl-PL")).AddYears(1)
				});
			}
			DbManager.InsertHolidays(holidays);

			driver.Close();
		}
	}
}
