using Entities;

namespace Services
{
    public interface IUserService
    {
        int CheckPassword(string password);
        User Login(string userName, string password);
        User Register(User user);
        User UpDate(User user, int id);
    }
}