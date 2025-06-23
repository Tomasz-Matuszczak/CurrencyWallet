using CurrencyWallet.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateProvider _currencyRateProvider;

        public CurrencyRateController(ICurrencyRateProvider currencyRateProvider)
        {
            _currencyRateProvider = currencyRateProvider;
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrencyRates()
        {
            return Ok(await _currencyRateProvider.GetCurrencyRatesAsync());
        }
    }
}