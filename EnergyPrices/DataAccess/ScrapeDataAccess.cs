namespace EnergyPrices.DataAccess;

public interface IScrapeDataAccess
{
    Task<string> ScrapeNordPool(DateOnly date);
}

public class ScrapeDataAccess : IScrapeDataAccess
{
    public async Task<string> ScrapeNordPool(DateOnly date)
    {
        HttpClient client = new HttpClient();
        string datestring = date.ToShortDateString();
        string url = $"https://www.nordpoolgroup.com/api/marketdata/page/41?currency=,DKK,DKK,DKK&endDate={datestring}";
        string result = await client.GetStringAsync(url);
        return result;
    }
}