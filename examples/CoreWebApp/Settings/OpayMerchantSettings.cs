namespace CoreWebApp.Settings
{
    public class OpayMerchantSettings
    {
        public string BaseUrl { get; set; }
        public string MerchantId { get; set; }
        public string PrivateKey { get; set; }
        public int? Timeout { get; set; }
        public string Iv { get; set; }
    }
}