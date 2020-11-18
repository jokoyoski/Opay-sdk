using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpayCashier.Models;
using Manager;
using OpayCashier.Models.Exceptions;

namespace OpayCashier
{
    /// <summary>
    /// This is the entry point to the OPay Cashier SDK. It holds configuration and state common
    /// to all APIs exposed from the SDK.
    /// </summary>
    public class CashierService : ICashierService
    {
        private readonly string _privateKey;
        private readonly HttpClient _httpClient;
        private readonly string _publicKey;

        private const string OrderPath = "api/v3/cashier/initialize";
        private const string StatusPath = "api/v3/cashier/status";
        private const string ClosePath = "api/v3/cashier/close";

        private const string JsonMediaType = "application/json";


        public CashierService()
        {

        }

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
        /// <param name="merchantId">Merchant ID</param>
        /// <param name="publicKey">Merchant public key</param>
        /// <param name="timeout">HTTP request timeout</param>
        public CashierService(string baseUrl, string merchantId, string publicKey, string privateKey,
            TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(merchantId))
            {
                throw new ArgumentNullException(nameof(merchantId));
            }

            if (string.IsNullOrWhiteSpace(publicKey))
            {
                throw new ArgumentNullException(nameof(publicKey));
            }

            if (string.IsNullOrWhiteSpace(privateKey))
            {
                throw new ArgumentNullException(nameof(privateKey));
            }

            _privateKey = privateKey;
            _publicKey = publicKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = timeout ?? TimeSpan.FromSeconds(5)
            };
            _httpClient.DefaultRequestHeaders.Add("MerchantId", merchantId);
        }

        /// <summary>
        /// Sends an order to the OPay cashier service to charge the user wallet.
        /// The request gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<OrderData>> Order(OrderRequest request)
        {
            request.Validate();
            var jsonRequest = JsonConvert.SerializeObject(request);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _publicKey);

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(OrderPath, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest, Encoding.UTF8, JsonMediaType)
            });
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpayCashierHttpException(response.StatusCode, jsonResponse);
            }

            return JsonConvert.DeserializeObject<BaseResponse<OrderData>>(jsonResponse);
        }

        /// <summary>
        /// Sends a query to the OPay cashier service to query existing transaction.
        /// The request gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<QueryData>> Query(QueryRequest request)
        {
            request.Validate();
            var jsonRequest = JsonConvert.SerializeObject(request);

            var signature = Helper.EncryptData(jsonRequest, _privateKey);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", signature);

            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(StatusPath, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest, Encoding.UTF8, JsonMediaType)
            });
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpayCashierHttpException(response.StatusCode, jsonResponse);
            }

            return JsonConvert.DeserializeObject<BaseResponse<QueryData>>(jsonResponse);
        }

        /// <summary>
        /// Close an order transaction on the OPay cashier service.
        /// The order gets validated both by the SDK, and the remote OPay service.
        /// A successful return value indicates that the message has been successfully sent and completed.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BaseResponse<CloseData>> Close(CloseRequest request)
        {
            request.Validate();
            var jsonRequest = JsonConvert.SerializeObject(request);

            var signature = Helper.EncryptData(jsonRequest,_privateKey);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", signature);
            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                RequestUri = new Uri(ClosePath, UriKind.Relative),
                Method = HttpMethod.Post,
                Content = new StringContent(jsonRequest, Encoding.UTF8, JsonMediaType)
            });
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new OpayCashierHttpException(response.StatusCode, jsonResponse);
            }

            return JsonConvert.DeserializeObject<BaseResponse<CloseData>>(jsonResponse);
        }

        
    }
}