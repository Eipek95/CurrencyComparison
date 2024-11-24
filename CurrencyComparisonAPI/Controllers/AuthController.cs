using Business.Abstract;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;
using Utilities;

namespace CurrencyComparisonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(IUserService userService, IJwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {

            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Geçersiz kullanıcı adı veya şifre" });
            }

            var token = _jwtTokenService.GenerateToken(user.Username);
            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FullName
                }
            });
        }
    }
}
