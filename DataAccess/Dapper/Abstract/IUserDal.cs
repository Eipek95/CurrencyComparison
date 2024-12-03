using Entities;
using SharedLibrary.DTOs;

namespace DataAccess.Dapper.Abstract
{
    public interface IUserDal
    {
        Task<Response<UserModel>> GetUserByUsernameAsync(string username);
    }
}
