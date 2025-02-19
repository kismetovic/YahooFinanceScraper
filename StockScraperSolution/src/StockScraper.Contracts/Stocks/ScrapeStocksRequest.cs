namespace StockScraper.Contracts.Stocks
{
    public record ScrapeStocksRequest(
        DateTime dateTime,
        string Ticker);
}
