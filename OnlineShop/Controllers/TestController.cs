using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using OnlineShop.Shared.Controllers;

namespace OnlineShop.AuthService.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ApiBaseController
    {
        

        public TestController()
        {

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Route("test")]
        public async Task<string> CheckTest()
        {
            return ResultOk("Test check");

        }
    }
}