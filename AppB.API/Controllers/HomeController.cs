using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppB.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Authorize]
        [HttpPost("{sayi}")]
        public object Calculate(int sayi)
        {
            var result = new
            {
                Result = sayi * sayi
            };

            return result;
        }
    }
}
