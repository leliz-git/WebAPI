using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        List<User> GetUsers();
        User Login(string userName);
        User Register(User user);
        User UpDate(User user, int id);
    }
}