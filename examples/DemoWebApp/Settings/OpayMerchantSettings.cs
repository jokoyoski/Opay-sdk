namespace DemoWebApp.Settings
{
    public class OpayCashierSettings
    {
        public string BaseUrl { get; set; }
        public string MerchantId { get; set; }
        public string PrivateKey { get; set; }
        public int? Timeout { get; set; }
        public string Iv { get; set; }
    }
}