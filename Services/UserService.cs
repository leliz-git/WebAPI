using Entities;

using Repositories;
using Zxcvbn;
namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public async Task<User>Register(User user)
        {
            //return userRepository.Register(user);
            if (CheckPassword(user.password) < 2)
            {
                return null;
            }
            List<User> users = await _userRepository.GetUsers();
            User userfound = users.FirstOrDefault(u => u.userName == user.userName);
            if (userfound == null)
            {
                return await _userRepository.Register(user);
            }
            return null;
        }
        public async Task<User> Login(string userName, string password)
        {

            User userfound = await _userRepository.Login(userName);
            if (userfound == null)
            {
                return null;
            }
            if (userfound.password.Trim() == password)
            {
                return userfound;
            }
            return null;
        }
        public async Task<User> UpDate(User user, int id)
        {

            return await _userRepository.UpDate(user, id);
        }
        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;


        }
    }
}
