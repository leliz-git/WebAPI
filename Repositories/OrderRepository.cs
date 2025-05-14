using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        MyShop_327882650Context dbContext;
        public OrderRepository(MyShop_327882650Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order> CreateOrder(Order order)
        {
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            return order;
        }
    }
}
