using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpayCashier;
using OpayCashier.Models;
using OpayCashier.Tests;

namespace DemoWebApp.Controllers
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
            return await _cashierService.Order(ServiceTests.MockOrderRequest);
        }
    }
}