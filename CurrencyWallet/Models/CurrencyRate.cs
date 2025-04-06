namespace CurrencyWallet.Models
{
    public class CurrencyRate
    {
        public required string Currency { get; set; }

        public required string Code { get; set; }

        public required decimal Mid { get; set; }
    }

}