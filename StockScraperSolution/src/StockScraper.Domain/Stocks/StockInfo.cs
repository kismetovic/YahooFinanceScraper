using StockScraper.Domain.Common.Models;
using StockScraper.Domain.Common.ValueObjects;

namespace StockScraper.Domain.Stocks
{
    public sealed class StockInfo : AggregateRoot<Guid>
    {
        public string? Ticker { get; private set; }
        public string? CompanyName { get; private set; }
        public MarketCap? MarketCap { get; private set; }
        public int YearFounded { get; private set; }
        public int NumberOfEmployees { get; private set; }
        public Location? Headquarters { get; private set; }
        public StockPrice? Price { get; private set; }
        public DateTime DateRetrieved { get; private set; }
        public DateTime DateScraped { get; private set; }

        private StockInfo() { }

        public StockInfo(Guid id, string ticker, string companyName, MarketCap marketCap, int yearFounded, int numberOfEmployees, Location headquarters, StockPrice price, DateTime dateRetrieved, DateTime dateScraped)
            : base(id)
        {
            Ticker = ticker;
            CompanyName = companyName;
            MarketCap = marketCap;
            YearFounded = yearFounded;
            NumberOfEmployees = numberOfEmployees;
            Headquarters = headquarters;
            Price = price;
            DateRetrieved = dateRetrieved;
            DateScraped = dateScraped;
        }
    }

}
