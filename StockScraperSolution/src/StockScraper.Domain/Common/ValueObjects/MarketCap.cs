using StockScraper.Domain.Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StockScraper.Domain.Common.ValueObjects
{
    public sealed class MarketCap : ValueObject
    {
        public decimal Value { get; }
        public string Currency { get; }

        public MarketCap(decimal value, string currency)
        {
            if (value < 0) throw new ArgumentException("Market Cap cannot be negative.");
            Value = value;
            Currency = currency;
        }

        public static MarketCap Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Market Cap cannot be null or empty.");

            Match match = Regex.Match(input, @"([\d.]+)([A-Za-z]*)");

            if (!match.Success)
                throw new FormatException("Invalid Market Cap format.");

            decimal numericValue = decimal.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            string currencySuffix = match.Groups[2].Value;

            return new MarketCap(numericValue, currencySuffix);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Currency;
        }
    }

}
