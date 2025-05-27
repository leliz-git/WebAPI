using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class OrderReositoryunittesting
    {
        [Fact]
        public async Task CreateOrder_AddsOrderAndReturnsIt()
        {
            // Arrange
            var orders = new List<Order> { new Order { OrderId = 2, OrderDate = System.DateTime.Now } };

            var mockContext = new Mock<MyShop_327882650Context>();
            mockContext.Setup(x => x.Orders).ReturnsDbSet(orders);

            var repository = new OrderRepository(mockContext.Object);
            var newOrder = new Order { OrderId = 1, OrderDate = System.DateTime.Now };

            // Act
            var result = await repository.CreateOrder(newOrder);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newOrder.OrderId, result.OrderId);
            
            //Assert.Single(mockContext.Object.Orders);
            //Assert.Contains(result, o => o.OrderId == 1);
            mockContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
            mockContext.Verify(x => x.Orders.AddAsync(newOrder, default), Times.Once);

        }
    }
}
