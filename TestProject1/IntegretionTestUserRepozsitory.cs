using Xunit;
using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using TestProject1;

namespace TestProject
{
    public class IntegrationTestUserRepository : IClassFixture<DatabaseFixture>
    {
        private readonly MyShop_327882650Context _dbContext;
        private readonly UserRepository _repo;

        public IntegrationTestUserRepository(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context; // שימוש ב-Context מה-fixture
            _repo = new UserRepository(_dbContext);
        }

        [Fact]
        public async Task Register_Should_Add_User()
        {
            // ניקוי טבלה לפני תחילת הטסט
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var user = new User
            {
                userName = "testuser",
                firstName = "Test",
                lastName = "User",
                password = "1234"
            };

            var result = await _repo.Register(user);

            Assert.NotNull(result);
            Assert.Equal("testuser", result.userName);

            var fromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.userName == "testuser");
            Assert.NotNull(fromDb);
            Assert.Equal("Test", fromDb.firstName);
        }

        [Fact]
        public async Task Register_WithMissingOptionalFields_Should_Add_User()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var user = new User
            {
                userName = "missingfields",
                password = "pass"
            };

            var result = await _repo.Register(user);

            Assert.NotNull(result);
            Assert.Equal("missingfields", result.userName);

            var fromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.userName == "missingfields");
            Assert.NotNull(fromDb);
            Assert.Null(fromDb.firstName);
            Assert.Null(fromDb.lastName);
        }

        //[Fact]   להוסיף בדטה בייס בסמינר שזה יהיה יחודי
        //public async Task Register_ExistingUserName_Should_FailOrThrow()
        //{
        //    _dbContext.Users.RemoveRange(_dbContext.Users);
        //    await _dbContext.SaveChangesAsync();

        //    var user = new User { userName = "dupuser", password = "a" };
        //    await _repo.Register(user);

        //    var duplicate = new User { userName = "dupuser", password = "b" };
        //    await Assert.ThrowsAnyAsync<DbUpdateException>(async () => await _repo.Register(duplicate));
        //}

        [Fact]
        public async Task Login_Should_Return_User_When_Exists()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var user = new User { userName = "login_user", password = "pass" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var result = await _repo.Login("login_user");
            Assert.NotNull(result);
            Assert.Equal("login_user", result.userName);
        }

        [Fact]
        public async Task Login_NonExistingUser_Should_Return_Null()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var result = await _repo.Login("does_not_exist");
            Assert.Null(result);
        }

        [Fact]
        public async Task Update_Should_Change_User_Details()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var user = new User { userName = "to_update", firstName = "Before", lastName = "Orig", password = "pass" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var updateData = new User { userName = "updated", firstName = "After", lastName = null, password = "newpass" };
            var result = await _repo.UpDate(updateData, user.userId);

            Assert.NotNull(result);
            Assert.Equal("updated", result.userName);
            Assert.Equal("After", result.firstName);
            Assert.Equal("Orig", result.lastName); // לא משתנה כי התקבל null
            Assert.Equal("newpass", result.password);
        }

        [Fact]
        public async Task Update_WithNullFields_Should_Keep_Old_Values()
        {
            // ניקוי טבלת המשתמשים
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            // יצירת משתמש ראשוני עם ערכים
            var user = new User { userName = "keep", firstName = "First", lastName = "Last", password = "Pass" };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            // עדכון המשתמש עם אובייקט שכל השדות בו null
            var updateData = new User();
            var result = await _repo.UpDate(updateData, user.userId);

            Assert.NotNull(result);

            // שליפת המשתמש מחדש מהדאטהבייס כדי לוודא שהערכים לא השתנו
            var userFromDb = await _dbContext.Users.FirstOrDefaultAsync(u => u.userId == user.userId);

            Assert.NotNull(userFromDb);
            Assert.Equal("keep", userFromDb.userName);
            Assert.Equal("First", userFromDb.firstName);
            Assert.Equal("Last", userFromDb.lastName);
            Assert.Equal("Pass", userFromDb.password);
        }

        [Fact]
        public async Task Update_NotExistingUser_Should_Return_Null()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var updateData = new User { userName = "none", firstName = "none", lastName = "none", password = "none" };
            var result = await _repo.UpDate(updateData, -1);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsers_Should_Return_All()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var u1 = new User { userName = "u1", password = "1" };
            var u2 = new User { userName = "u2", password = "2" };
            await _dbContext.Users.AddRangeAsync(u1, u2);
            await _dbContext.SaveChangesAsync();

            var users = await _repo.GetUsers();
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);
            Assert.Contains(users, u => u.userName == "u1");
            Assert.Contains(users, u => u.userName == "u2");
        }

        [Fact]
        public async Task GetAllUsers_Should_Return_Empty_WhenNone()
        {
            _dbContext.Users.RemoveRange(_dbContext.Users);
            await _dbContext.SaveChangesAsync();

            var users = await _repo.GetUsers();
            Assert.NotNull(users);
            Assert.Empty(users);
        }
    }
}