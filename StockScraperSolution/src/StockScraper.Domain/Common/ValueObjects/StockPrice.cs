using StockScraper.Domain.Common.Models;

namespace StockScraper.Domain.Common.ValueObjects
{
    public sealed class StockPrice : ValueObject
    {
        public decimal PreviousClose { get; }
        public decimal Open { get; }

        public StockPrice(decimal previousClose, decimal open)
        {
            if (previousClose < 0 || open < 0)
                throw new ArgumentException("Stock prices cannot be negative.");
            PreviousClose = previousClose;
            Open = open;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PreviousClose;
            yield return Open;
        }
    }


}
