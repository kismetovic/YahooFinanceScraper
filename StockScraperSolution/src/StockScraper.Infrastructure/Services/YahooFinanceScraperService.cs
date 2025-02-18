using ErrorOr;
using MediatR;
using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Common.ValueObjects;
using StockScraper.Domain.Stocks;
using StockScraper.Infrastructure.Common.Errors;
using StockScraper.Infrastructure.Common.Models;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace StockScraper.Infrastructure.Services
{
    public sealed class YahooFinanceScraperService : IYahooFinanceScraperService
    {
        private readonly YahooFinanceSettings _settings;

        public YahooFinanceScraperService(IOptions<YahooFinanceSettings> options)
        {
            _settings = options.Value;
        }

        public ErrorOr<StockInfo> ScrapeStockDataAsync(string ticker, DateTime date)
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

            var remoteWebDriverUrl = Environment.GetEnvironmentVariable("SELENIUM_REMOTE_URL") ?? "http://selenium-hub:4444/wd/hub";

            var driver = new RemoteWebDriver(new Uri(remoteWebDriverUrl), options);

            //using (var driver = new ChromeDriver(chromeDriverService, options))
            //{
                string summaryUrl = $"{_settings.BaseUrl}{_settings.SummaryPath.Replace("{ticker}", ticker)}";
                string profileUrl = $"{_settings.BaseUrl}{_settings.ProfilePath.Replace("{ticker}", ticker)}";

                var navigateSummaryResult = NavigateToUrl(driver, summaryUrl);
                if (navigateSummaryResult.IsError)
                {
                    driver.Quit();
                    return navigateSummaryResult.Errors;
                }

                var companyNameElement = FindElement(driver, _settings.XPaths["CompanyName"]);
                var marketCapElement = FindElement(driver, _settings.XPaths["MarketCap"]);

                if (companyNameElement.IsError || marketCapElement.IsError)
                {
                    driver.Quit();
                    return companyNameElement.IsError ? companyNameElement.Errors : marketCapElement.Errors;
                }

                string companyName = companyNameElement.Value.Text;
                string marketCap = marketCapElement.Value.Text;

                var navigateProfileResult = NavigateToUrl(driver, profileUrl);
                if (navigateProfileResult.IsError)
                {
                    driver.Quit();
                    return navigateProfileResult.Errors;
                }

                var countryElement = FindElement(driver, _settings.XPaths["Country"]);
                var cityElement = FindElement(driver, _settings.XPaths["City"]);
                var employeesElement = FindElement(driver, _settings.XPaths["Employees"]);
                var descriptionElement = FindElement(driver, _settings.XPaths["CompanyDescription"]);

                if (countryElement.IsError || cityElement.IsError || employeesElement.IsError || descriptionElement.IsError)
                {
                    driver.Quit();
                    return countryElement.IsError ? countryElement.Errors :
                           cityElement.IsError ? cityElement.Errors :
                           employeesElement.IsError ? employeesElement.Errors : descriptionElement.Errors;
                }

                string state = countryElement.Value.Text;
                string city = cityElement.Value.Text;
                string numOfEmployees = employeesElement.Value.Text;
                string companyDescription = descriptionElement.Value.Text;

                string historyUrl = _settings.BaseUrl + _settings.HistoryPath
                    .Replace("{ticker}", ticker)
                    .Replace("{period1}", ((DateTimeOffset)date).ToUnixTimeSeconds().ToString())
                    .Replace("{period2}", ((DateTimeOffset)date.AddDays(1)).ToUnixTimeSeconds().ToString());

                var navigateHistoryResult = NavigateToUrl(driver, historyUrl);
                if (navigateHistoryResult.IsError)
                {
                    driver.Quit();
                    return navigateHistoryResult.Errors;
                }

                var openElement = FindElement(driver, _settings.XPaths["OpenPrice"]);
                var closeElement = FindElement(driver, _settings.XPaths["ClosePrice"]);

                if (openElement.IsError || closeElement.IsError)
                {
                    driver.Quit();
                    return openElement.IsError ? openElement.Errors : closeElement.Errors;
                }

                string open = openElement.Value.Text;
                string close = closeElement.Value.Text;

                if (!decimal.TryParse(open, out decimal openDecimal))
                {
                    driver.Quit();
                    return Errors.Scraper.InvalidData("Failed to parse open price.");
                }

                if (!decimal.TryParse(close, out decimal closeDecimal))
                {
                    driver.Quit();
                    return Errors.Scraper.InvalidData("Failed to parse close price.");
                }

                if (!int.TryParse(numOfEmployees, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int numberOfEmployees))
                {
                    driver.Quit();
                    return Errors.Scraper.InvalidData("Failed to parse number of employees.");
                }

                var marketCapValue = MarketCap.Parse(marketCap);
                var location = new Location(city, state);
                var stockPrice = new StockPrice(closeDecimal, openDecimal);
                var yearFounded = ExtractFoundedYear(companyDescription);

                if (yearFounded.IsError)
                {
                    driver.Quit();
                    return yearFounded.Errors;
                }

                driver.Quit();

                return new StockInfo(Guid.NewGuid(), ticker, companyName, marketCapValue, yearFounded.Value, numberOfEmployees, location, stockPrice, date, DateTime.UtcNow);
            //}
        }

        private static ErrorOr<IWebElement> FindElement(IWebDriver driver, string xpath)
        {
            try
            {
                var element = driver.FindElement(By.XPath(xpath)).ToErrorOr();
                return element;
            }
            catch (NoSuchElementException)
            {
                return Errors.Scraper.ElementNotFound($"Element not found with XPath: {xpath}");
            }
            catch (Exception ex)
            {
                return Errors.Scraper.Unexpected($"Unexpected error while finding element: {ex.Message}");
            }
        }

        private static ErrorOr<Unit> NavigateToUrl(IWebDriver driver, string url)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                return Errors.Scraper.NavigationFailed($"Failed to navigate to URL: {url}. Error: {ex.Message}");
            }
        }

        private static ErrorOr<int> ExtractFoundedYear(string text)
        {
            Match match = Regex.Match(text, @"\b(?:founded|incorporated)\s+in\s+(\d{4})\b", RegexOptions.IgnoreCase);

            if (match.Success)
                return int.Parse(match.Groups[1].Value);

            return Errors.Scraper.ElementNotFound("Element not found for Year Founded.");
        }
    }
}