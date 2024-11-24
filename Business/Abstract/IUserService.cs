using Business.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<UserDtoModel> GetUserByUsernameAsync(string username);
    }
}
