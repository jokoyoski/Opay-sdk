namespace OpayCashier.Models
{
    public class PayAmount
    {
        public PayAmount(float value, string currency)
        {
            Value = value;
            Currency = currency;
        }

        public float Value { get; }
        public string Currency { get; }
    }
}