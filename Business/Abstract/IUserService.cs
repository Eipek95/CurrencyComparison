using Business.Dtos;
using SharedLibrary.DTOs;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<Response<UserDtoModel>> GetUserByUsernameAsync(LoginRequestDto request);
    }
}
