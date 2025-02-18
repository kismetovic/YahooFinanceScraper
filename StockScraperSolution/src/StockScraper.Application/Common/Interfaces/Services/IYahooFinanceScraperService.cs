using StockScraper.Domain.Stocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Application.Common.Interfaces.Services
{
    public interface IYahooFinanceScraperService
    {
        StockInfo ScrapeStockDataAsync(string ticker, DateTime date);
    }
}
