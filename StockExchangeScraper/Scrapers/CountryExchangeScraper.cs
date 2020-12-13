using OpenQA.Selenium;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;

namespace StockExchangeScraper
{
	class CountryExchangeScraper : Scraper
	{
		public void DownloadCountriesExchanges()
		{
			string url = "https://www.investing.com/stock-screener/";
			driver.Url = url;
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//div[contains(@data-filter-type, 'country')]/a[@class='newBtnDropdown noHover']/i")).Click();

			var countriesUL = driver.FindElements(By.XPath("//ul[@id='countriesUL']/li"));
			for (int i = 0; i < countriesUL.Count; i++)
			{
				IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
				js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", countriesUL[i]);

				Country country = new Country { Name = countriesUL[i].Text };

				countriesUL[i].Click();

				driver.FindElement(By.XPath("//div[contains(@data-filter-type, 'exchange')]/a[@class='newBtnDropdown noHover']/i")).Click();
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

				List<string> exchanges = new List<string>();
				var exchangesUL = driver.FindElements(By.XPath("//ul[@id='exchangesUL']/li"));
				for (int j = 1; j < exchangesUL.Count; j++)
				{
					exchanges.Add(exchangesUL[j].Text.Split(" (")[0]);
				}

				DbManager.InsertCountryExchange(country, exchanges);

				driver.FindElement(By.XPath("//div[contains(@data-filter-type, 'country')]/a[@class='newBtnDropdown noHover']/i")).Click();
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
			}
		}
	}
}
