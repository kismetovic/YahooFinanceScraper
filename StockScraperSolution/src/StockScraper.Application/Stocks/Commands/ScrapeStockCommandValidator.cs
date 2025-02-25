﻿using FluentValidation;

namespace StockScraper.Application.Stocks.Commands
{
    public sealed class ScrapeStockCommandValidator : AbstractValidator<ScrapeStockCommand>
    {
        public ScrapeStockCommandValidator()
        {
            RuleFor(x => x.Ticker)
                .NotNull().WithMessage("Ticker is required")
                .Length(1, 10).WithMessage("Ticker must be between 1 and 10 characters.");

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
        }
    }
}
