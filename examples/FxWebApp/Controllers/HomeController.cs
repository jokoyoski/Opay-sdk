using System;
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
            var privateKey = ConfigurationManager.AppSettings[""];
            var iv = ConfigurationManager.AppSettings[""];
            _cashierService = new CashierService(baseUrl, merchantId, privateKey, iv, TimeSpan.FromSeconds(5));
        }

        public async Task<BaseResponse<OrderResponse>> Order()
        {
            return await _cashierService.Order(null);
        }

        public async Task<BaseResponse<CloseResponse>> About()
        {
            return await _cashierService.Close(null);
        }

        public async Task<BaseResponse<QueryResponse>> Contact()
        {
            return await _cashierService.Query(null);
        }
    }
}