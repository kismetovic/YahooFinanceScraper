﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StockScraper.Application.Common.Interfaces.Persistance;
using StockScraper.Application.Common.Interfaces.Services;
using StockScraper.Infrastructure.Common.Models;
using StockScraper.Infrastructure.Persistance;
using StockScraper.Infrastructure.Persistance.Repositories;
using StockScraper.Infrastructure.Services;

namespace StockScraper.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddPersistance(configuration);
            services.Configure<YahooFinanceSettings>(configuration.GetSection("YahooFinance"));
            services.AddSingleton<IYahooFinanceScraperService, YahooFinanceScraperService>();
            return services;
        }

        public static IServiceCollection AddPersistance(this IServiceCollection services, ConfigurationManager configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<StockScraperDbContext>(options => options.UseSqlServer(connectionString, options =>
            {
                options.MigrationsAssembly("StockScraper.Infrastructure");
                options.EnableRetryOnFailure();
            }));

            services.AddScoped<IStockRepository, StockRepository>();
            return services;
        }
    }
}
