using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockScraper.Contracts.Stocks
{
    public sealed record StockResponse(
        Guid Id,
        string Ticker,
        string CompanyName,
        decimal PreviousClosePrice,
        decimal OpenPrice,
        string MarketCap,
        int YearFounded,
        int NumberOfEmployees,
        string HeadquartersCity,
        string HeadquartersState,
        DateTime DateRetrieved);
}
