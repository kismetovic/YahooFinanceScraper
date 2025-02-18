using Mapster;
using StockScraper.Application.Stocks.Commands;
using StockScraper.Application.Stocks.Queries;
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
                .Map(dest => dest.Ticker, src => src.Ticker);

            config.NewConfig<StockInfo, StockResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Ticker, src => src.Ticker)
                .Map(dest => dest.CompanyName, src => src.CompanyName)
                .Map(dest => dest.PreviousClosePrice, src => src.Price!.PreviousClose)
                .Map(dest => dest.OpenPrice, src => src.Price!.Open)
                .Map(dest => dest.MarketCap, src => src.MarketCap!.Value)
                .Map(dest => dest.MarketCapCurrency, src => src.MarketCap!.Currency)
                .Map(dest => dest.YearFounded, src => src.YearFounded)
                .Map(dest => dest.NumberOfEmployees, src => src.NumberOfEmployees)
                .Map(dest => dest.HeadquartersCity, src => src.Headquarters!.City)
                .Map(dest => dest.HeadquartersState, src => src.Headquarters!.State)
                .Map(dest => dest.DateRetrieved, src => src.DateRetrieved)
                .Map(dest => dest.DateScraped, src => src.DateScraped);


            config.NewConfig<IEnumerable<StockInfo>, GetAllStocksResponse>();

        }
    }
}
