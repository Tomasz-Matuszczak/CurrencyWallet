namespace CurrencyWallet.Models
{
    public class CurrencyRate
    {
        public required string Currency { get; set; }
        public required string Code { get; set; }
        public required decimal Mid { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (CurrencyRate)obj;
            return Currency == other.Currency && Code == other.Code && Mid == other.Mid;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Currency, Code, Mid);
        }
    }
}