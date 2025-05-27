using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    class userRepositoryUnittesting
    {
      
        public class UserRepositoryUnitTesting
        {
            [Fact]
            public async Task GetUsers_ReturnsAllUsers()
            {
                // Arrange
                var users = new List<User>
            {
                new User { userId = 1, userName = "user1", firstName = "A", lastName = "B", password = "pass1" },
                new User { userId = 2, userName = "user2", firstName = "C", lastName = "D", password = "pass2" }
            };
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                // Act
                var result = await repository.GetUsers();

                // Assert
                Assert.Equal(2, result.Count);
                Assert.Contains(result, u => u.userName == "user1");
                Assert.Contains(result, u => u.userName == "user2");
            }

            [Fact]
            public async Task Register_AddsUserAndReturnsIt()
            {
                // Arrange
                var users = new List<User>();
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                var newUser = new User { userId = 1, userName = "newuser", firstName = "New", lastName = "User", password = "pass" };

                // Act
                var result = await repository.Register(newUser);

                // Assert
                Assert.Equal(newUser, result);
                mockContext.Verify(x => x.Users.AddAsync(newUser, default), Times.Once);
                mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
            }

            [Fact]
            public async Task Login_ReturnsUser_WhenExists()
            {
                // Arrange
                var users = new List<User>
            {
                new User { userId = 1, userName = "loginUser", password = "pass" }
            };
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                // Act
                var result = await repository.Login("loginUser");

                // Assert
                Assert.NotNull(result);
                Assert.Equal("loginUser", result.userName);
            }

            [Fact]
            public async Task Login_ReturnsNull_WhenNotExists()
            {
                // Arrange
                var users = new List<User>();
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                // Act
                var result = await repository.Login("doesNotExist");

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task UpDate_UpdatesExistingUser()
            {
                // Arrange
                var existingUser = new User { userId = 1, userName = "oldUser", firstName = "Old", lastName = "Name", password = "oldpass" };
                var users = new List<User> { existingUser };
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                var updateUser = new User { userName = "newUser", firstName = "New", lastName = "Name", password = "newpass" };

                // Act
                var result = await repository.UpDate(updateUser, 1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("newUser", result.userName);
                Assert.Equal("New", result.firstName);
                Assert.Equal("Name", result.lastName);
                Assert.Equal("newpass", result.password);
                mockContext.Verify(x => x.Users.Update(result), Times.Once);
                mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
            }

            [Fact]
            public async Task UpDate_ReturnsNull_WhenUserNotExists()
            {
                // Arrange
                var users = new List<User>();
                var mockContext = new Mock<MyShop_327882650Context>();
                mockContext.Setup(x => x.Users).ReturnsDbSet(users);

                var repository = new UserRepository(mockContext.Object);

                var updateUser = new User { userName = "doesntMatter" };

                // Act
                var result = await repository.UpDate(updateUser, 999);

                // Assert
                Assert.Null(result);
            }
        }
    }
}

