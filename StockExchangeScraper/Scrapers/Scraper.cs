using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace StockExchangeScraper
{
	abstract class Scraper
	{
		protected IWebDriver driver;

		public Scraper()
		{
			ChromeOptions options = new ChromeOptions();
			options.AddArguments("--ignore-certificate-errors", "--incognito", //"--headless",
								 "--window-size=1920,1080", "--log-level=3",
								 "--disable-blink-features=AutomationControlled",
								 "--no-sandbox");

			driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(AppDomain.CurrentDomain.BaseDirectory), options, TimeSpan.FromMinutes(5));

			//driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, options);
		}
	}
}
