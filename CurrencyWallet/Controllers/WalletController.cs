using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyWallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly IWalletServices _walletServices;

        public WalletController(IWalletServices walletServices)
        {
            _walletServices = walletServices;
        }

        [HttpPost("AddFunds")]
        public IActionResult AddMoneyToWallet(int id, [FromBody] WalletTransaction transaction)
        {
            try { 
                _walletServices.AddMoneyToWallet(id, transaction.CurrencyCode.ToUpper(), Math.Round(transaction.Amount, 2));
                return Ok("Add funds to user wallet successful.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Withdraw")]
        public IActionResult WithdrawMoneyFromWallet(int id, [FromBody] WalletTransaction transaction)
        {
            try
            {
                _walletServices.WithdrawMoneyFromWallet(id, transaction.CurrencyCode.ToUpper(), Math.Round(transaction.Amount,2));
                return Ok("Withdraw operation successful.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Exchange")]
        public IActionResult ExchangeCurrency(int id, [FromBody] CurrencyExchange exchange)
        {
            try
            {
                _walletServices.ExchangeCurrency(id, exchange.FromCurrency.ToUpper(), exchange.ToCurrency.ToUpper(), Math.Round(exchange.Amount, 2));
                return Ok($"Exchange operation successful.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
