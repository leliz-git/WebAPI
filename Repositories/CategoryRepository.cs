using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        MyShop_327882650Context dbContext;
        public CategoryRepository(MyShop_327882650Context dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await dbContext.Categories.ToListAsync();
        }
    }
}
