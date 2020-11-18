namespace OpayCashier.Models
{
    public class ApiRequest
    {
        public ApiRequest(string merchantId, string data)
        {
            MerchantId = merchantId;
            Data = data;
        }

        public string MerchantId { get; }
        public string Data { get; }
    }
}