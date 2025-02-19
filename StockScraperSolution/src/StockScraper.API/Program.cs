using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using StockScraper.API.Common.Mappings;
using StockScraper.Application;
using StockScraper.Infrastructure;
using StockScraper.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
        // This helps surviving a restart: a same app will find back its keys. Just ensure to create the folder.
        .PersistKeysToFileSystem(new DirectoryInfo("\\MyKeys\\"))
        // This helps surviving a site update: each app has its own store, building the site creates a new app
        .SetApplicationName("StockScraper")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddMappings();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("StockScraper");
    });
}

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StockScraperDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
