using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Common.ValueObjects;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockScraper.Infrastructure.Services
{
    public sealed class YahooFinanceScraperService : IYahooFinanceScraperService
    {
        private readonly HttpClient _httpClient;
        public YahooFinanceScraperService()
        {
            this._httpClient = new HttpClient();
        }
        public async Task<StockInfo> ScrapeStockDataAsync(string ticker, DateTime date)
        {
            // Set up ChromeDriver service to suppress logs
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.SuppressInitialDiagnosticInformation = true; // Suppress ChromeDriver logs
            chromeDriverService.HideCommandPromptWindow = true; // Hide the command prompt window

            // Set up Chrome options to suppress browser logs
            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3"); // Set log level to suppress most logs
            options.AddArgument("--disable-logging"); // Disable logging
            options.AddArgument("--silent"); // Run in silent mode

            // Initialize ChromeDriver
            using (var driver = new ChromeDriver(chromeDriverService, options))
            {
                try
                {
                    string summary = $"https://finance.yahoo.com/quote/{ticker}";

                    driver.Navigate().GoToUrl(summary);

                    var companyNameElement = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/section[1]/div[1]/div/div/section/h1"));
                    var marketCapElement = driver.FindElement(By.CssSelector("fin-streamer.yf-gn3zu3[data-field='marketCap']"));

                    string companyName = companyNameElement.Text;
                    string marketCap = marketCapElement.Text;


                    string profile = $"https://finance.yahoo.com/quote/{ticker}/profile";

                    driver.Navigate().GoToUrl(profile);

                    var countryElement = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/section[2]/section[2]/div/div/div/div[3]"));
                    var companyCity = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/section[2]/section[2]/div/div/div/div[2]"));
                    var numberOfEmployeesElement = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/section[2]/section[2]/div/dl/div[3]/dd/strong"));

                    string state = countryElement.Text;
                    string city = companyCity.Text;
                    string numOfEmployees = numberOfEmployeesElement.Text;

                    DateTime testDate = new DateTime(2025, 02, 13);
                    string url3 = $"https://finance.yahoo.com/quote/{ticker}/history/?period1={((DateTimeOffset)testDate).ToUnixTimeSeconds()}&period2={((DateTimeOffset)testDate.AddDays(1)).ToUnixTimeSeconds().ToString()}";

                    driver.Navigate().GoToUrl(url3);
                    var openElement = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/div[1]/div[3]/table/tbody/tr/td[2]"));
                    var closeElement = driver.FindElement(By.XPath("//*[@id='nimbus-app']/section/section/section/article/div[1]/div[3]/table/tbody/tr/td[5]"));

                    string open = openElement.Text;
                    string close = closeElement.Text;

                    decimal.TryParse(marketCap, out decimal marketCapDecimal);
                    decimal.TryParse(open, out decimal openDecimal);
                    decimal.TryParse(close, out decimal closeDecimal);
                    
                    int.TryParse(numOfEmployees, out int numberOfEmployees);
                    
                    var marketCapValue = new MarketCap(marketCapDecimal);
                    
                    var location = new Location(city, state);

                    var stockPrice = new StockPrice(closeDecimal, openDecimal);

                    return new StockInfo(Guid.NewGuid(), ticker, companyName, marketCapValue, 2000, numberOfEmployees, location, stockPrice, date);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    driver.Quit();
                }
            }
        }

        
    }
}
