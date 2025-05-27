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

        public async Task<List<Product>> GetProducts(string?desc, int?minprice, int?maxprice, int?[]categoriesId)
        {
            var query = dbContext.Products.Include(product => product.Category)
       .Where(product =>
           (desc == null ? true : product.Description.Contains(desc)) &&
           (minprice == null ? true : product.Price >= minprice) &&
           (maxprice == null ? true : product.Price <= maxprice) &&
           (categoriesId.Length == 0 ? true : categoriesId.Contains(product.CategoryId))
       )
       .OrderBy(product => product.Price);

            List<Product> products = await query.ToListAsync();

            return products;
        }
    }
}
