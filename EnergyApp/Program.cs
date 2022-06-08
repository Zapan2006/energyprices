using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EnergyApp;
using EnergyApp.DataAccess;
using EnergyApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<IPriceService, PriceService>();
builder.Services.AddScoped<IScrapeDataAccess, ScrapeDataAccess>();


WebAssemblyHost host = builder.Build();
await host.RunAsync();