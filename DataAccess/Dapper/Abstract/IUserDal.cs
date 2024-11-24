using Entities;

namespace DataAccess.Dapper.Abstract
{
    public interface IUserDal
    {
        Task<UserModel> GetUserByUsernameAsync(string username);
    }
}
