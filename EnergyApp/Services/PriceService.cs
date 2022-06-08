using System.Globalization;
using EnergyApp.DataAccess;
using EnergyApp.Models;
using Newtonsoft.Json.Linq;

namespace EnergyApp.Services;

public interface IPriceService
{
    Task<List<DataItem>> ScrapePrices(DateOnly date);
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
}