using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace StockExchangeScraper
{
	class HistoricalQuotesScraper : Scraper
	{
		public void DownloadHistorical(string indexName = "WIG", string start = "01/01/2015", string end = "10/19/2020")
		{
			string url = "https://investing.com/equities/poland";
			driver.Url = url;
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			var select = new SelectElement(driver.FindElement(By.Id("stocksFilter")));
			select.SelectByText(indexName);
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			var rows = driver.FindElements(By.XPath("//table[@id='cross_rate_markets_stocks_1']/tbody/tr"));
			if (rows.Count > 0)
			{
				var stockList = rows.Select(r => r.FindElement(By.XPath("./td[2]/a")).GetAttribute("href")).ToList();

				DownloadStocksHistory(stockList, start, end);
			}

			driver.Close();
		}

		public void DownloadStocksHistory(List<string> stockList, string start, string end)
		{
			foreach (var stock in stockList)
			//for (int i = 30; i < stockList.Count; i++)
			{
				if (DbManager.StockIsToDownload(stock, start, end) == false)
				{
					continue;
				}

				driver.Url = GetUrl(stock);
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				var select = new SelectElement(driver.FindElement(By.Id("data_interval")));
				select.SelectByValue("Daily");
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
				js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});arguments[0].click()", driver.FindElement(By.Id("widgetFieldDateRange")));
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				var startDate = driver.FindElement(By.Id("startDate"));
				startDate.Clear();
				startDate.SendKeys(start);

				var endDate = driver.FindElement(By.Id("endDate"));
				endDate.Clear();
				endDate.SendKeys(end);

				driver.FindElement(By.Id("applyBtn")).Click();
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				var rows = driver.FindElements(By.XPath("//table[contains(@class, 'historicalTbl') and @id='curr_table']/tbody/tr"));

				List<StockQuote> history = GetTable(rows);

				DbManager.InsertStockQuote(history);
			}
		}

		private string GetUrl(string url)
		{
			if (url.Contains('?'))
			{
				int index = url.IndexOf('?');
				return url.Insert(index, "-historical-data");
			}
			return $"{url}-historical-data";
		}

		private List<StockQuote> GetTable(IEnumerable<IWebElement> rows)
		{
			List<StockQuote> stocks = new List<StockQuote>();

			if (rows.Count() == 1 && rows.First().Text == "No results found")
			{
				return stocks;
			}

			foreach (var row in rows)
			{
				DateTime.TryParseExact(row.FindElement(By.XPath("./td[1]")).Text, "MMM dd, yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

				stocks.Add(new StockQuote
				{
					CompanyIsin = driver.FindElement(By.XPath("//span[text()='ISIN:']/following-sibling::span[@class='elp']")).Text.Trim(),
					Open = Parse.Double(row.FindElement(By.XPath($"./td[3]")).Text, ".", ","),
					Close = Parse.Double(row.FindElement(By.XPath($"./td[2]")).Text, ".", ","),
					Min = Parse.Double(row.FindElement(By.XPath($"./td[5]")).Text, ".", ","),
					Max = Parse.Double(row.FindElement(By.XPath($"./td[4]")).Text, ".", ","),
					Volume = Format.Volume(row.FindElement(By.XPath($"./td[6]")).Text),
					Date = date
				});
			}
			return stocks;
		}
	}
}
