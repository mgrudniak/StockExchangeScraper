using OpenQA.Selenium;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;

namespace StockExchangeScraper
{
	class QuotesScraper : Scraper
	{
		private readonly List<StockQuote> stocks = new List<StockQuote>();

		public void DownloadWarsaw(string indexName = "WIG")
		{
			if (IsHoliday() == false)
			{
				Console.WriteLine($"[{DateTime.Now}]\tStarted\n");

				string url = "https://www.gpw.pl/akcje";
				driver.Url = url;
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
				js.ExecuteScript("arguments[0].scrollIntoView();arguments[0].click()", driver.FindElement(By.XPath("//div[contains(@class,'extraFiltersList')]/div/a")));
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

				driver.FindElement(By.XPath("//div[contains(@class,'extraFiltersList')]/div/ul/li/label[1]")).Click();
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

				driver.FindElement(By.XPath("//div[contains(@class,'extraFiltersList')]/div/ul//ul/li[5]/label")).Click();
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				driver.FindElement(By.XPath("//div[contains(@class,'extraFiltersList')]/div/a")).Click();

				var rows = driver.FindElements(By.XPath($"//div[@class='table-responsive']/table/tbody/tr[contains(@data-indeks, '{indexName}')]"));
				foreach (var row in rows)
				{
					StockQuote stock = GetInfo(row);
					if (stock.CompanyIsin != "DE000A13SX89" || stock.Open != null || stock.Min != null || stock.Max != null)
					{
						stocks.Add(stock);
					}
				}
				DbManager.InsertStockQuote(stocks);

				Console.WriteLine($"[{DateTime.Now}]\tDone!\n");
			}
			driver.Close();
		}

		private bool IsHoliday()
		{
			return DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
				DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
				DbManager.IsHoliday();
		}

		private StockQuote GetInfo(IWebElement row)
		{
			return new StockQuote
			{
				CompanyIsin = row.FindElement(By.XPath("./td[3]/a")).GetProperty("href").Split("?isin=")[1],
				Open = Parse.Double(row.FindElement(By.XPath($"./td[9]")).Text, ",", " "),
				Close = Parse.Double(row.FindElement(By.XPath($"./td[12]")).Text, ",", " "),
				Min = Parse.Double(row.FindElement(By.XPath($"./td[10]")).Text, ",", " "),
				Max = Parse.Double(row.FindElement(By.XPath($"./td[11]")).Text, ",", " "),
				Volume = Parse.Int(row.FindElement(By.XPath($"./td[22]")).Text),
				Date = GetDate()
			};
		}

		private DateTime GetDate()
		{
			DateTime dateTime = DateTime.Now;
			if (dateTime.Hour >= 0 && dateTime.Hour < 7)
			{
				return DateTime.Now.AddDays(-1).Date;
			}
			if (dateTime.Hour > 17 && dateTime.Hour <= 23)
			{
				return DateTime.Now.Date;
			}
			throw new InvalidTimeException();
		}
	}
}
