using HtmlAgilityPack;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Domain.Common.ValueObjects;
using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Infrastructure.Services
{
    public sealed class YahooFinanceScraperService : IYahooFinanceScraperService
    {
        public async Task<StockInfo> ScrapeStockDataAsync(string ticker, DateTime date)
        {
            var url = $"https://finance.yahoo.com/quote/{ticker}";
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);

            var companyName = doc.DocumentNode.SelectSingleNode("//h1")?.InnerText ?? "Unknown";
            var marketCap = new MarketCap(500000000, "USD");
            var yearFounded = 1990;
            var numberOfEmployees = 10000;
            var headquarters = new Location("New York", "NY");
            var price = new StockPrice(150.00m, 152.00m);

            return new StockInfo(Guid.NewGuid(), ticker, companyName, marketCap, yearFounded, numberOfEmployees, headquarters, price, date);
        }
    }
}
