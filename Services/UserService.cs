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
        public User Register(User user)
        {
            //return userRepository.Register(user);
            if (CheckPassword(user.password) < 2)
            {
                return null;
            }
            List<User> users = _userRepository.GetUsers();
            User userfound = users.FirstOrDefault(u => u.userName == user.userName);
            if (userfound == null)
            {
                return _userRepository.Register(user);
            }
            return null;
        }
        public User Login(string userName, string password)
        {

            User userfound = _userRepository.Login(userName);
            if (userfound == null)
            {
                return null;
            }
            if (userfound.password == password)
            {
                return userfound;
            }
            return null;
        }
        public User UpDate(User user, int id)
        {

            return _userRepository.UpDate(user, id);
        }
        public int CheckPassword(string password)
        {
            var result = Zxcvbn.Core.EvaluatePassword(password);
            return result.Score;


        }
    }
}
