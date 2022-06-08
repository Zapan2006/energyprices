using System.Globalization;
using EnergyPrices.DataAccess;
using EnergyPrices.Models;
using Newtonsoft.Json.Linq;

namespace EnergyPrices.Services;

public interface IPriceService
{
    Task<List<DataItem>> ScrapePrices(DateOnly date);
    Task<List<DataItem>> ScrapePrices(DateOnly date, int bestAmount);
}

public class PriceService : IPriceService
{
    private IScrapeDataAccess _scrapeDataAccess;

    public PriceService(IScrapeDataAccess scrapeDataAccess)
    {
        _scrapeDataAccess = scrapeDataAccess;
    }

    public async Task<List<DataItem>> ScrapePrices(DateOnly date)
    {
        string dataString = await _scrapeDataAccess.ScrapeNordPool(date);
        JObject data = JObject.Parse(dataString);

        List<DataItem> list = data.SelectTokens("$..Rows[?(true)]")
            .Where(token => Convert.ToDateTime(token.SelectToken("$.EndTime").ToString()).Hour != 0)
            .Select(token =>
                new DataItem
                {
                    StartTime = Convert.ToDateTime(token.SelectToken("$.StartTime").ToString()).Hour,
                    EndTime = Convert.ToDateTime(token.SelectToken("$.EndTime").ToString()).Hour,
                    Value = Convert.ToDecimal(token.SelectToken("$.Columns[?(@.Name == 'DK2')].Value").ToString().Replace(" ", ""), new CultureInfo("da-DK"))
                }
            ).ToList();

        return list;
    }

    public async Task<List<DataItem>> ScrapePrices(DateOnly date, int bestAmount)
    {
        List<DataItem> scrapePrices = await ScrapePrices(date);
        scrapePrices = scrapePrices.OrderBy(price => price.Value).ToList();
        foreach (DataItem price in scrapePrices.Take(bestAmount))
        {
            price.Highlighted = true;
        }

        return scrapePrices.OrderBy(price => price.StartTime).ToList();
    }
}