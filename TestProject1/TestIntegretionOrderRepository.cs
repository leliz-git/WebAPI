using Xunit;
using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TestProject1; // לפי המקום בו DatabaseFixture שלך נמצא

namespace TestProject
{
    public class IntegrationTestOrderRepository : IClassFixture<DatabaseFixture>
    {
        private readonly MyShop_327882650Context _dbContext;
        private readonly OrderRepository _repo;

        public IntegrationTestOrderRepository(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _repo = new OrderRepository(_dbContext);
        }

        private async Task<int> SeedUserAsync()
        {
            var user = new User
            {
                userName = "testuser",
                firstName = "Test",
                lastName = "User",
                password = "1234"
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.userId;
        }

        // בדיקה: יצירת הזמנה תקינה
        [Fact]
        public async Task Create_ValidOrder_ShouldAddOrder()
        {
            // Arrange
            var userId = await SeedUserAsync();
            var order = new Order
            {
                OrderDate = DateTime.Today,
                OrderSum = 250,
                UserId = userId
            };

            // Act
            var result = await _repo.CreateOrder(order);

            // Assert
            Assert.NotNull(result);
            var addedOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == result.OrderId);
            Assert.NotNull(addedOrder);
            Assert.Equal(250, addedOrder.OrderSum);
            Assert.Equal(userId, addedOrder.UserId);
        }

        // בדיקה: יצירת הזמנה עם שדות ריקים/לא תקינים
        [Fact]
        public async Task Create_InvalidOrder_ShouldFail()
        {
            // בדיקה על אובייקט ריק
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _repo.CreateOrder(null);
            });

            // בדיקה על שדות חסרים/לא תקינים
            var order = new Order
            {
                // אין OrderDate, אין UserId, אין OrderSum
            };

            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await _repo.CreateOrder(order);
            });
        }
    }
}