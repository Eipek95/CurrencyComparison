using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyComparisonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _userService.GetUserByUsernameAsync(request);
            return ActionResultInstance(result);
        }
    }
}
