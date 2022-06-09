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
        public async Task<List<DataItem>> Get([FromQuery]string date, int? amount = null)
        {
            DateOnly dateOnly = DateOnly.Parse(date);
            if(amount == null)
                return await _priceService.ScrapePrices(dateOnly).ConfigureAwait(false);
            else
                return await _priceService.ScrapePrices(dateOnly, amount.GetValueOrDefault()).ConfigureAwait(false);
        }
    }
}
