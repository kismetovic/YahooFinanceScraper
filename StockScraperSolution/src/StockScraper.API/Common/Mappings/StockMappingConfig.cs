using Mapster;
using StockScraper.Application.Stocks.Commands;
using StockScraper.Contracts.Stocks;
using StockScraper.Domain.Stocks;

namespace StockScraper.API.Common.Mappings
{
    public class StockMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config) 
        {
            config.NewConfig<ScrapeStocksRequest, ScrapeStockCommand>()
                .Map(dest => dest.Date, src => src.dateTime)
                .Map(dest => dest.Tickers, src => src.Tickers);

            config.NewConfig<StockInfo, StockResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Ticker, src => src.Ticker)
                .Map(dest => dest.CompanyName, src => src.CompanyName)
                .Map(dest => dest.PreviousClosePrice, src => src.Price!.PreviousClose)
                .Map(dest => dest.OpenPrice, src => src.Price!.Open)
                .Map(dest => dest.MarketCap, src => $"{src.MarketCap!.Value} {src.MarketCap.Currency}")
                .Map(dest => dest.YearFounded, src => src.YearFounded)
                .Map(dest => dest.NumberOfEmployees, src => src.NumberOfEmployees)
                .Map(dest => dest.HeadquartersCity, src => src.Headquarters!.City)
                .Map(dest => dest.HeadquartersState, src => src.Headquarters!.State)
                .Map(dest => dest.DateRetrieved, src => src.DateRetrieved);

        }
    }
}
