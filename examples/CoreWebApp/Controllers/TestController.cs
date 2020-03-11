using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpayCashier;
using OpayCashier.Models;

namespace CoreWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICashierService _cashierService;

        public TestController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

        [HttpGet("order")]
        public async Task<BaseResponse<OrderResponse>> Order()
        {
            var order = new OrderRequest
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
            return await _cashierService.Order(order);
        }
    }
}