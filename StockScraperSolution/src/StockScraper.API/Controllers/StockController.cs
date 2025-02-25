﻿using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockScraper.Application.Stocks.Commands;
using StockScraper.Application.Stocks.Queries;
using StockScraper.Contracts.Stocks;

namespace StockScraper.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public StockController(IMapper mapper, ISender mediator)
        {
            this._mapper = mapper;
            this._mediator = mediator;
        }

        [HttpPost("/scrape")]
        public async Task<IActionResult> ScrapeStocks(ScrapeStocksRequest request)
        {
            var command = _mapper.Map<ScrapeStockCommand>(request);

            var scrapeStockResult = await _mediator.Send(command);


            return scrapeStockResult.Match(
                stock => Ok(_mapper.Map<StockResponse>(scrapeStockResult.Value)),
                errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var query = new GetAllStocksQuery();

            var getAllResult = await _mediator.Send(query);

            var result = getAllResult.Select(si => _mapper.Map<StockResponse>(si));

            return Ok(result);
        }
    }
}
