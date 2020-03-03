using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpayCashier.Models;

namespace OpayCashier
{
    /// <summary>
    /// This is the entry point to the OPay Cashier SDK. It holds configuration and state common
    /// to all APIs exposed from the SDK.
    /// </summary>
    public class CashierService : ICashierService
    {
        private readonly string _merchantId;
        private readonly string _privateKey;
        private readonly string _iv;
        private readonly string _baseUrl;
        private readonly TimeSpan _timeout;

        private const string OrderPath = "api/cashier/order";
        private const string StatusPath = "api/cashier/merchantOrderStatus";
        private const string ClosePath = "api/cashier/merchantCloseOrder";

        static CashierService()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                },
            };
        }


        /// <summary>
        /// Creates a new OPay cashier service.
        /// </summary>
        /// <param name="baseUrl">The base URL for Opay cashier services</param>
        /// <param name="merchantId">Current merchant ID</param>
        /// <param name="privateKey">Encryption private key</param>
        /// <param name="iv">Encryption initialization vector</param>
        /// <param name="timeout">HTTP request timeout</param>
        public CashierService(string baseUrl, string merchantId, string privateKey, string iv, TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            if (string.IsNullOrWhiteSpace(iv))
            {
                throw new ArgumentNullException(nameof(iv));
            }

            if (!Url.IsValid(baseUrl))
            {
                throw new ArgumentException("invalid base url", nameof(baseUrl));
            }

            _baseUrl = baseUrl;
            _timeout = timeout ?? TimeSpan.FromSeconds(5);
            _merchantId = merchantId;
            _privateKey = privateKey;
            _iv = iv;
        }

        /// <summary>
        /// Sends an order to the OPay cashier service to charge the user wallet.
        /// The request gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<OrderResponse>> Order(OrderRequest request)
        {
            var jsonString = JsonConvert.SerializeObject(request);

            return await _baseUrl
                .AppendPathSegment(OrderPath)
                .WithTimeout(_timeout)
                .PostJsonAsync(new ApiRequest(_merchantId, Encrypt(jsonString)))
                .ReceiveJson<BaseResponse<OrderResponse>>();
        }

        /// <summary>
        /// Sends a query to the OPay cashier service to query existing transaction.
        /// The request gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<QueryResponse>> Query(QueryRequest request)
        {
            var jsonString = JsonConvert.SerializeObject(request);

            return await _baseUrl
                .AppendPathSegment(StatusPath)
                .WithTimeout(_timeout)
                .PostJsonAsync(new ApiRequest(_merchantId, Encrypt(jsonString)))
                .ReceiveJson<BaseResponse<QueryResponse>>();
        }

        /// <summary>
        /// Close an order transaction on the OPay cashier service.
        /// The order gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CloseResponse>> Close(CloseRequest request)
        {
            var jsonString = JsonConvert.SerializeObject(request);

            return await _baseUrl
                .AppendPathSegment(ClosePath)
                .WithTimeout(_timeout)
                .PostJsonAsync(new ApiRequest(_merchantId, Encrypt(jsonString)))
                .ReceiveJson<BaseResponse<CloseResponse>>();
        }

        private string Encrypt(string data)
        {
            byte[] cipherBytes;
            using (var aes = new RijndaelManaged())
            {
                var plainBytes = Encoding.UTF8.GetBytes(data);
                aes.Key = Encoding.UTF8.GetBytes(_privateKey);
                aes.IV = Encoding.UTF8.GetBytes(_iv);
                using (var encrypt = aes.CreateEncryptor())
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherBytes = ms.ToArray();
                        }
                    }
                }
            }

            return Convert.ToBase64String(cipherBytes);
        }
    }
}