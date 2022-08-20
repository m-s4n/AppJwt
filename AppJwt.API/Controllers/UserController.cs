using AppJwt.Core.Dtos;
using AppJwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using AppJwt.API.Helpers;
using SharedLibrary.Exceptions;

namespace AppJwt.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            //throw new CustomException("Bir hata meydana geldi");
            var result = await _userService.CreateUserAsync(createUserDto);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }

        // Token ile korunan action
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            dynamic tokenInfo = TokenOperator.DecodeToken(HttpContext.Request.Headers[HeaderNames.Authorization]);
            var result = await _userService.GetUserByNameAsync(tokenInfo.Name);

            return new ObjectResult(result)
            {
                StatusCode = result.StatusCode
            };
        }
    }
}
