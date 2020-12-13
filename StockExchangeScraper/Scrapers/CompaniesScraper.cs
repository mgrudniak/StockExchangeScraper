using OpenQA.Selenium;
using StockExchangeScraper.Models;
using System;
using System.Collections.Generic;

namespace StockExchangeScraper
{
	class CompaniesScraper : Scraper
	{
		public void DownloadCompanies(string country = "Poland", string exchange = "Warsaw")
		{
			List<Company> companies = GetCompaniesInfo(country, exchange);

			foreach (var company in companies)
			{
				//if (DataLibrary.DbManager.CompanyIsToDownload(company.Url) == false)
				//{
				//	continue;
				//}

				driver.Url = company.Url;
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

				var companyProfile = driver.FindElements(By.XPath("//div[@class='companyProfileHeader']"));
				if (companyProfile.Count > 0)
				{
					company.EmployeesNumber = Parse.Int(companyProfile[0].FindElement(By.XPath("./div[text()='Employees']/p")).Text);
					company.EquityType = new EquityType { Name = companyProfile[0].FindElement(By.XPath("./div[text()='Equity Type']/p")).Text };
				}

				company.Isin = driver.FindElement(By.XPath("//span[text()='ISIN:']/following-sibling::span[@class='elp']")).Text.Trim();
				company.Name = driver.FindElement(By.XPath("//div[@class='instrumentHead']")).Text.Split(" (")[0];

				DbManager.InsertCompanies(company);
			}

			driver.Close();
		}

		private List<Company> GetCompaniesInfo(string country = "Poland", string exchange = "Warsaw")
		{
			string url = "https://www.investing.com/stock-screener/";
			driver.Url = url;
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//div[contains(@data-filter-type, 'country')]/a[@class='newBtnDropdown noHover']/i")).Click();
			driver.FindElement(By.XPath($"//li[contains(@data-flag-value, '{country}')]")).Click();
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//div[contains(@data-filter-type, 'exchange')]/a[@class='newBtnDropdown noHover']/i")).Click();
			driver.FindElement(By.XPath($"//li[contains(text(), '{exchange}')]")).Click();
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//span[@class='colSelectIcon']")).Click();
			System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

			driver.FindElement(By.XPath("//input[@value='last']")).Click();
			driver.FindElement(By.XPath("//input[@value='sector_trans']")).Click();
			driver.FindElement(By.XPath("//input[@value='industry_trans']")).Click();
			driver.FindElement(By.Id("selectColumnsButton_stock_screener")).Click();

			List<Company> companies = new List<Company>();
			bool firstIter = true;
			do
			{
				if (firstIter == false)
				{
					driver.FindElement(By.XPath($"//div[@id='paginationWrap']/div[3]/a")).Click();
					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
				}
				else firstIter = false;

				var table = driver.FindElements(By.XPath("//table[@id='resultsTable']/tbody/tr"));
				foreach (var row in table)
				{
					companies.Add(GetCompaniesFromTable(row));
				}
			} while (driver.FindElements(By.XPath($"//div[@id='paginationWrap']/div[3]/a")).Count > 0);

			return companies;
		}

		private Company GetCompaniesFromTable(IWebElement row)
		{
			return new Company
			{
				Exchange = new Exchange { Name = row.FindElement(By.XPath("./td[4]")).Text },
				Sector = new Sector { Name = row.FindElement(By.XPath("./td[5]")).Text },
				Industry = new Industry { Name = row.FindElement(By.XPath("./td[6]")).Text },
				Url = row.FindElement(By.XPath("./td[2]/a")).GetAttribute("href")
			};
		}
	}
}
