namespace OpayCashier.Models
{
    public class OrderData : BaseData
    {
        public string CashierUrl { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
}