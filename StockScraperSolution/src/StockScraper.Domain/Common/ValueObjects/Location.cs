using StockScraper.Domain.Common.Models;

namespace StockScraper.Domain.Common.ValueObjects
{
    public sealed class Location : ValueObject
    {
        public string City { get; }
        public string State { get; }

        public Location(string city, string state)
        {
            City = city;
            State = state;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return City;
            yield return State;
        }
    }


}
