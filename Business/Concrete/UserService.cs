using Business.Abstract;
using Business.Dtos;
using DataAccess.Dapper.Abstract;
using SharedLibrary.DTOs;
using System.Net;
using Utilities;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly IJwtTokenService _jwtTokenService;

        public UserService(IUserDal userDal, IJwtTokenService jwtTokenService)
        {
            _userDal = userDal;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<Response<UserDtoModel>> GetUserByUsernameAsync(LoginRequestDto request)
        {
            try
            {
                var userModel = await _userDal.GetUserByUsernameAsync(request.Username);

                if (userModel.Data == null)
                {
                    return Response<UserDtoModel>.Fail(new ErrorDto("Kullanıcı Bulunamadı", true), Convert.ToInt16(HttpStatusCode.NotFound));
                }
                if (!PasswordHasher.VerifyPassword(request.Password, userModel.Data.PasswordHash))
                {
                    return Response<UserDtoModel>.Fail(new ErrorDto("Kullanıcı Adı Veya Şifre Yanlış", true), Convert.ToInt16(HttpStatusCode.BadRequest));
                }

                var token = _jwtTokenService.GenerateToken(userModel.Data.Username);
                var userDto = new UserDtoModel()
                {
                    Id = userModel.Data.Id,
                    Email = userModel.Data.Email,
                    FullName = userModel.Data.FullName,
                    PasswordHash = userModel.Data.PasswordHash,
                    Username = userModel.Data.Username,
                    Token = token
                };

                return Response<UserDtoModel>.Success(userDto, Convert.ToInt16(HttpStatusCode.OK));
            }
            catch (Exception ex)
            {
                return Response<UserDtoModel>.Fail(new ErrorDto($"Bir hata oluştu: {ex.Message}", true), Convert.ToInt16(HttpStatusCode.InternalServerError));
            }
        }
    }
}
