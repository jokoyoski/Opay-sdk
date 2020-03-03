using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpayCashier.Models;
using Xunit;
using Xunit.Abstractions;

namespace OpayCashier.Tests
{
    public class ServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly CashierService _cashierService;
        private const string BaseUrl = "http://api.test.opaydev.com:8081";
        private const string MerchantId = "256619112122000";
        private const string PrivateKey = "fKJ8jwsj1nHNkKon";
        private const string Iv = "qazwert12345!@#$";

        public ServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _cashierService = new CashierService(BaseUrl, MerchantId, PrivateKey, Iv, TimeSpan
                .FromSeconds(3)
            );
        }

        [Fact]
        public async Task TestOrder()
        {
            var orderResponse = await _cashierService.Order(MockOrderRequest);
          _outputHelper.WriteLine(JsonConvert.SerializeObject(MockOrderRequest));
          _outputHelper.WriteLine(JsonConvert.SerializeObject(orderResponse));

            Assert.Equal("00000", orderResponse.Code);
        }

        [Fact]
        public void TestQuery()
        {
            // _cashierService.Query()
        }

        public static readonly OrderRequest MockOrderRequest = new OrderRequest
        {
            PaymentMethods = new List<PaymentMethod>
            {
                PaymentMethod.Account,
                PaymentMethod.Card,
                PaymentMethod.QrCode
            },
            Reference = "test_20201123102233",
            ProductDesc = "The best wireless earphone in history. Cannot agree more! Right!",
            PayChannels = new List<PayChannel>
            {
                PayChannel.BalancePayment,
                PayChannel.BonusPayment
            },
            PayAmount = new PayAmount(10.0F, "NGN"),
            UserMobile = "+2349876543210",
            UserRequestIp = "154.113.66.58",
            CallbackUrl = "http://XXXXXXXXXXXXX/callback",
            ReturnUrl = "http://XXXXXXXXXXXXXXX/return",
            MchShortName = "Jerry's shop",
            ProductName = "Apple AirPods Pro"
        };
    }
}