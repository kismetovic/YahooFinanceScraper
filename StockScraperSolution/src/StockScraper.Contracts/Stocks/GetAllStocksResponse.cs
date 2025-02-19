namespace StockScraper.Contracts.Stocks
{
    public sealed record GetAllStocksResponse(IEnumerable<StockResponse> responses);
}
