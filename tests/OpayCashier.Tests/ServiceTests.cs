using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
        private const string BaseUrl = "https://cashierapi.operapay.com";
        private const string MerchantId = "256619092316009";
        private const string PublicKey = "OPAYPUB15692645578840.8244955863818603";
        private const string PrivateKey = "OPAYPRV15692645578850.9045063711017022";

        public ServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _cashierService = new CashierService(BaseUrl, MerchantId, PublicKey, PrivateKey, TimeSpan.FromSeconds(3));
        }


        [Fact]
        public async Task TestOrder()
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

            var orderResponse = await _cashierService.Order(orderRequest);

            Assert.Equal("00000", orderResponse.Code);
            Assert.Equal("SUCCESSFUL", orderResponse.Message);
            Assert.NotNull(orderResponse.Data);
            Assert.Equal(orderRequest.Amount, orderResponse.Data.Amount);
            Assert.NotEmpty(orderResponse.Data.OrderNo);
            Assert.NotEmpty(orderResponse.Data.Reference);
            Assert.Equal("SUCCESS", orderResponse.Data.Status);
        }

        [Fact]
        public async Task TestQuery()
        {
            var queryRequest = new QueryRequest
            {
                OrderNo = "200408141665571675",
                Reference = "test_637219088240003560"
            };

            var queryResponse = await _cashierService.Query(queryRequest);
            
            Assert.Equal("00000", queryResponse.Code);
            Assert.Equal("SUCCESSFUL", queryResponse.Message);
            Assert.NotNull(queryResponse.Data);
            Assert.NotEmpty(queryResponse.Data.Amount);
            Assert.NotEmpty(queryResponse.Data.OrderNo);
            Assert.NotEmpty(queryResponse.Data.Reference);
            Assert.Equal("INITIAL", queryResponse.Data.Status);
        }

        [Fact]
        public async Task TestClose()
        {
            var closeRequest = new CloseRequest
            {
                OrderNo = "200408141665571675",
                Reference = "test_637219088240003560"
            };

            var closeResponse = await _cashierService.Close(closeRequest);
            
            Assert.Equal("00000", closeResponse.Code);
            Assert.Equal("SUCCESSFUL", closeResponse.Message);
            Assert.NotNull(closeResponse.Data);
            Assert.NotEmpty(closeResponse.Data.OrderNo);
            Assert.NotEmpty(closeResponse.Data.Reference);
            Assert.Equal("CLOSED", closeResponse.Data.Status);
        }
    }
}