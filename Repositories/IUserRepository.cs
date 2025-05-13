using Entities;

namespace Repositories
{
    public interface IUserRepository
    {
        Task<List<User> >GetUsers();
        Task<User> Login(string userName);
        Task<User> Register(User user);
        Task<User> UpDate(User user, int id);
    }
}