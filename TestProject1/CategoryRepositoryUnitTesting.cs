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
    public class CategoryRepositoryUnitTesting
    {
        [Fact]
        public async Task GetCategories_ReturnsCategoriesWithProducts()
        {
            var categories = new List<Category>
            {
                new Category { CategoryId = 1, CategoryName = "a", Products = new List<Product> { new Product { ProductId = 1, ProductName = "a" } } },
                new Category { CategoryId = 2, CategoryName = "b", Products = new List<Product> { new Product { ProductId = 2, ProductName = "b" } } }
            };

            var mockContex = new Mock<MyShop_327882650Context>();
            mockContex.Setup(x => x.Categories).ReturnsDbSet(categories);

            var categoryRepository = new CategoryRepository(mockContex.Object);

            var result = await categoryRepository.GetCategories();

            //Assert.Equal(2, result.Count);
            Assert.NotNull(result);
            Assert.Contains(result, c => c.CategoryName == "a" && c.Products.Count == 1);
            Assert.Contains(result, c => c.CategoryName == "b" && c.Products.Count == 1);
        }
    }
}
