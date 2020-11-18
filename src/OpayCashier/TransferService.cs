using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OpayCashier.Models.Exceptions;
using Manager.Validators;

namespace OpayCashier
{
    public class TransferService :ITransferService
    {
        private readonly string _privateKey;
        private readonly HttpClient _httpClient;
        private readonly string _publicKey;
        private readonly string _merchantId;
        private const string TransferPath = "api/v3/transfer/status/toWallet";
      
        private const string JsonMediaType = "application/json";



        static TransferService()
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
        public TransferService(string baseUrl, string merchantId, string privateKey, string publicKey,
            TimeSpan? timeout = null)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
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
            _merchantId = merchantId;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = timeout ?? TimeSpan.FromSeconds(5)
            };
          
        }

        public async Task<BaseResponse<TransferResponse>> Transfer(TransferRequest request)
        {
            request.ValidateRequest();
            var jsonRequest = JsonConvert.SerializeObject(request);
            var signature = Helper.EncryptData(jsonRequest, _privateKey);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signature);

           
                
            var response = await _httpClient.SendAsync(new HttpRequestMessage
            {
                    RequestUri = new Uri(TransferPath, UriKind.Relative),
                    Method = HttpMethod.Post,
                    Content = new StringContent(jsonRequest, Encoding.UTF8, JsonMediaType)
            });
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
              throw new OpayCashierHttpException(response.StatusCode, jsonResponse);
            }

            return JsonConvert.DeserializeObject<BaseResponse<TransferResponse>>(jsonResponse);
            
            
           
        }
    }
}
