using StockScraper.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Domain.Common.ValueObjects
{
    public sealed class MarketCap : ValueObject
    {
        public decimal Value { get; }

        public MarketCap(decimal value)
        {
            if (value < 0) throw new ArgumentException("Market Cap cannot be negative.");
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

}
