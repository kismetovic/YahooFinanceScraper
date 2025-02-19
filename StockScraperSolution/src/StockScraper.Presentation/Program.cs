using Microsoft.AspNetCore.DataProtection;
using MudBlazor.Services;
using StockScraper.Presentation.Components;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataProtection()
        // This helps surviving a restart: a same app will find back its keys. Just ensure to create the folder.
        .PersistKeysToFileSystem(new DirectoryInfo("\\MyKeysPR\\"))
        // This helps surviving a site update: each app has its own store, building the site creates a new app
        .SetApplicationName("StockScraperPR")
        .SetDefaultKeyLifetime(TimeSpan.FromDays(90));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetConnectionString("API")!) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
