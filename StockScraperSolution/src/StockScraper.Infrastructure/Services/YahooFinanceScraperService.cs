using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Common.ValueObjects;
using StockScraper.Domain.Stocks;
using StockScraper.Infrastructure.Common.Models;
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
        private readonly YahooFinanceSettings _settings;

        public YahooFinanceScraperService(IOptions<YahooFinanceSettings> options)
        {
            _settings = options.Value;
        }

        public StockInfo ScrapeStockDataAsync(string ticker, DateTime date)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            chromeDriverService.HideCommandPromptWindow = true;


            var options = new ChromeOptions();
            options.AddArgument("--headless=new");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--log-level=3");
            options.AddArgument("--disable-logging");
            options.AddArgument("--silent");

            using (var driver = new ChromeDriver(chromeDriverService, options))
            {
                try
                {
                    string summaryUrl = $"{_settings.BaseUrl}{_settings.SummaryPath.Replace("{ticker}", ticker)}";
                    string profileUrl = $"{_settings.BaseUrl}{_settings.ProfilePath.Replace("{ticker}", ticker)}";

                    driver.Navigate().GoToUrl(summaryUrl);
                    var companyNameElement = driver.FindElement(By.XPath(_settings.XPaths["CompanyName"]));
                    var marketCapElement = driver.FindElement(By.XPath(_settings.XPaths["MarketCap"]));

                    string companyName = companyNameElement.Text;
                    string marketCap = marketCapElement.Text;

                    driver.Navigate().GoToUrl(profileUrl);
                    var countryElement = driver.FindElement(By.XPath(_settings.XPaths["Country"]));
                    var cityElement = driver.FindElement(By.XPath(_settings.XPaths["City"]));
                    var employeesElement = driver.FindElement(By.XPath(_settings.XPaths["Employees"]));
                    var descriptionElement = driver.FindElement(By.XPath(_settings.XPaths["CompanyDescription"]));

                    string state = countryElement.Text;
                    string city = cityElement.Text;
                    string numOfEmployees = employeesElement.Text;
                    string companyDescription = descriptionElement.Text;

                    string historyUrl = _settings.BaseUrl + _settings.HistoryPath
                        .Replace("{ticker}", ticker)
                        .Replace("{period1}", ((DateTimeOffset)date).ToUnixTimeSeconds().ToString())
                        .Replace("{period2}", ((DateTimeOffset)date.AddDays(1)).ToUnixTimeSeconds().ToString());

                    driver.Navigate().GoToUrl(historyUrl);
                    var openElement = driver.FindElement(By.XPath(_settings.XPaths["OpenPrice"]));
                    var closeElement = driver.FindElement(By.XPath(_settings.XPaths["ClosePrice"]));

                    string open = openElement.Text;
                    string close = closeElement.Text;

                    decimal.TryParse(open, out decimal openDecimal);
                    decimal.TryParse(close, out decimal closeDecimal);

                    int.TryParse(numOfEmployees, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int numberOfEmployees);
                    var marketCapValue = MarketCap.Parse(marketCap);
                    var location = new Location(city, state);
                    var stockPrice = new StockPrice(closeDecimal, openDecimal);
                    int yearFounded = ExtractFoundedYear(companyDescription);

                    return new StockInfo(Guid.NewGuid(), ticker, companyName, marketCapValue, yearFounded, numberOfEmployees, location, stockPrice, date, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error scraping stock data: {ex.Message}");
                }
                finally
                {
                    driver.Quit();
                }
            }
        }
        static int ExtractFoundedYear(string text)
        {
            Match match = Regex.Match(text, @"\b(?:founded|incorporated)\s+in\s+(\d{4})\b", RegexOptions.IgnoreCase);

            if (match.Success)
                return int.Parse(match.Groups[1].Value);

            return 0;
        }
    }
}
