using Dapper;
using DataAccess.Dapper.Abstract;
using DataAccess.Dapper.Contexts;
using Entities;

namespace DataAccess.Dapper.Concrete
{
    public class UserDal : IUserDal
    {
        private readonly DapperDbContext _dbContext;

        public UserDal(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            var query = "SELECT * FROM TBL_USER WHERE Username = @Username";
            using (var connection = _dbContext.Connection)
            {
                return await connection.QueryFirstOrDefaultAsync<UserModel>(query, new { Username = username });
            }

        }
    }
}
