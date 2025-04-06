namespace CurrencyWallet.Models
{
    public class CurrencyRate
    {
        public required string Currency { get; set; }

        public required string Code { get; set; }

        public required decimal Mid { get; set; }

        /*        public CurrencyRate(string currency, string code, decimal midRate)
                {
                    Currency = currency;
                    Code = code;
                    MidRate = midRate;
                }*/
    }

}