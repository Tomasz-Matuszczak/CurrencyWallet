using CurrencyWallet.Interfaces;
using CurrencyWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyWallet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : Controller
    {
        private readonly IUserRepository _userRepository;

        public WalletController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("{id}/addFunds")]
        public IActionResult AddMoneyToWallet(int id, [FromBody] WalletTransaction transaction)
        {
            try { 
                _userRepository.AddMoneyToWallet(id, transaction.CurrencyCode.ToUpper(), Math.Round(transaction.Amount, 2));
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/withdraw")]
        public IActionResult WithdrawMoneyFromWallet(int id, [FromBody] WalletTransaction transaction)
        {
            try
            {
                _userRepository.WithdrawMoneyFromWallet(id, transaction.CurrencyCode.ToUpper(), Math.Round(transaction.Amount,2));
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/exchange")]
        public IActionResult ExchangeCurrency(int id, [FromBody] CurrencyExchange exchange)
        {
            try
            {
                _userRepository.ExchangeCurrency(id, exchange.FromCurrency.ToUpper(), exchange.ToCurrency.ToUpper(), Math.Round(exchange.Amount, 2));
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
