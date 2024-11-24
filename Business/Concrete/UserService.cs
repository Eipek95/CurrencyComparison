using Business.Abstract;
using Business.Dtos;
using DataAccess.Dapper.Abstract;

namespace Business.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<UserDtoModel> GetUserByUsernameAsync(string username)
        {
            var userModel = await _userDal.GetUserByUsernameAsync(username);
            var userDto = new UserDtoModel()
            {
                Id = userModel.Id,
                Email = userModel.Email,
                FullName = userModel.FullName,
                PasswordHash = userModel.PasswordHash,
                Username = userModel.Username,
            };

            return userDto;
        }
    }
}
