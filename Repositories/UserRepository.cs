

using Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        MyShop_327882650Context dbContext;
        public UserRepository(MyShop_327882650Context dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<User>> GetUsers()
        {
            
            return await dbContext.Users.ToListAsync();
        }
        public async Task<User> Register(User user)
        {
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> Login(string userName)//(User user)
        {
            //Console.WriteLine(GetUsers());
            User user1 =await dbContext.Users.FirstOrDefaultAsync(u => u.userName == userName);
            return user1;


        }
        public async Task <User> UpDate(User user, int id)
        {

            //List<User> users = GetUsers();
            User userToUp = await dbContext.Users.FirstOrDefaultAsync(u => u.userId == id);
            if (userToUp == null)
            {
                return null;
            }
            userToUp.firstName = user.firstName != null ? user.firstName : userToUp.firstName;
            userToUp.lastName = user.lastName != null ? user.lastName : userToUp.lastName;
            userToUp.password = user.password != null ? user.password : userToUp.password;
            userToUp.userName = user.userName != null ? user.userName : userToUp.userName;
            dbContext.Users.Update(userToUp);
            await dbContext.SaveChangesAsync();
            return userToUp;
        }
       

    }
}
