using ErrorOr;
using StockScraper.Domain.Stocks;

namespace StockScraper.Application.Common.Interfaces.Services
{
    public interface IYahooFinanceScraperService
    {
        ErrorOr<StockInfo> ScrapeStockDataAsync(string ticker, DateTime date);
    }
}
