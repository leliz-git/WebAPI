using Entities;
using DTO;
using Repositories;
using Zxcvbn;
using AutoMapper;
namespace Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserDTO> Register(UserRegisterDTO user)
        {
            if (CheckPassword(user.password) < 2)
            {
                return null;
            }
            List<User> users = await _userRepository.GetUsers();
            User userfound = users.FirstOrDefault(u => u.userName == user.userName);
            if (userfound == null)
            {
                var user1 = _mapper.Map<User>(user);
                var userRegister = await _userRepository.Register(user1);

                return _mapper.Map<UserDTO>(userRegister);
            }
            return null;
        }

        public async Task<UserDTO> Login(UserLoginDTO user)
        {

            User userfound = await _userRepository.Login(user.userName);
            if (userfound == null)
            {
                return null;
            }
            //Console.WriteLine(userfound);
            if (userfound.password.Trim() == user.password)
            {
                return _mapper.Map<UserDTO>(userfound);
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
