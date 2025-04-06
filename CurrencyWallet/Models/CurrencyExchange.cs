namespace CurrencyWallet.Models
{
    public class CurrencyExchange
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
