using EnergyPrices.Models;
using EnergyPrices.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnergyPrices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet]
        public async Task<List<DataItem>> Get([FromQuery]string stringDate, int? bestAmount = null)
        {
            DateOnly date = DateOnly.Parse(stringDate);
            if(bestAmount == null)
                return await _priceService.ScrapePrices(date).ConfigureAwait(false);
            else
                return await _priceService.ScrapePrices(date, bestAmount.GetValueOrDefault()).ConfigureAwait(false);
        }
    }
}
