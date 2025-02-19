using ErrorOr;

namespace StockScraper.Infrastructure.Common.Errors
{
    public static class Errors
    {
        public static class Scraper
        {
            public static Error NotFound(string description) =>
                Error.NotFound(
                    code: "Scraper.NotFound",
                    description: description);

            public static Error Unexpected(string description) =>
                Error.Unexpected(
                    code: "Scraper.Unexpected",
                    description: description);

            public static Error InvalidData(string description) =>
                Error.Validation(
                    code: "Scraper.InvalidData",
                    description: description);

            public static Error NavigationFailed(string description) =>
                Error.Failure(
                    code: "Scraper.NavigationFailed",
                    description: description);

            public static Error ElementNotFound(string description) =>
                Error.NotFound(
                    code: "Scraper.ElementNotFound",
                    description: description);
        }
    }
}
