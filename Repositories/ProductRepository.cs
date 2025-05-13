using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        MyShop_327882650Context dbContext;
        public ProductRepository(MyShop_327882650Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await dbContext.Products.ToListAsync();
        }
    }
}
