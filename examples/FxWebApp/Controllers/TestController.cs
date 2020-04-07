using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using OpayCashier;
using OpayCashier.Models;

namespace FxWebApp.Controllers
{
    public class TestController : ApiController
    {
        private readonly ICashierService _cashierService;

        public TestController()
        {
            var baseUrl = ConfigurationManager.AppSettings[""];
            var merchantId = ConfigurationManager.AppSettings[""];
            var publicKey = ConfigurationManager.AppSettings[""];
            _cashierService = new CashierService(baseUrl, merchantId, publicKey, "", TimeSpan.FromSeconds(5));
        }

        public async Task<BaseResponse<OrderData>> Order()
        {
            var orderRequest = new OrderRequest
            {
                Reference = $"test_{DateTime.Now.Ticks}",
                MchShortName = "Jerry's shop",
                ProductName = "Apple AirPods Pro",
                ProductDesc = "The best wireless earphone in history. Cannot agree more! Right!",
                UserPhone = "+2349876543210",
                UserRequestIp = "154.113.66.58",
                Amount = "100",
                Currency = "NGN",
                PayTypes = new List<PayType>
                {
                    PayType.BalancePayment,
                    PayType.BonusPayment
                },
                PayMethods = new List<PayMethod>
                {
                    PayMethod.Account,
                    PayMethod.QrCode
                },
                CallbackUrl = "http://example.com/callback",
                ReturnUrl = "http://example.com/return",
                ExpireAt = "1"
            };

            return await _cashierService.Order(orderRequest);
        }

        public async Task<BaseResponse<QueryData>> Query()
        {
            var queryRequest = new QueryRequest
            {
                OrderNo = "200408141665571675",
                Reference = "test_637219088240003560"
            };

            return await _cashierService.Query(queryRequest);
        }

        public async Task<BaseResponse<CloseData>> Close()
        {
            var closeRequest = new CloseRequest
            {
                OrderNo = "200408141665571675",
                Reference = "test_637219088240003560"
            };

            return await _cashierService.Close(closeRequest);
        }
    }
}