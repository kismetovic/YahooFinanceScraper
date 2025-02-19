namespace StockScraper.Contracts.Stocks
{
    public sealed record StockResponse(
        Guid Id,
        string Ticker,
        string CompanyName,
        decimal PreviousClosePrice,
        decimal OpenPrice,
        decimal MarketCap,
        string MarketCapCurrency,
        int YearFounded,
        int NumberOfEmployees,
        string HeadquartersCity,
        string HeadquartersState,
        DateTime DateRetrieved,
        DateTime DateScraped);
}
