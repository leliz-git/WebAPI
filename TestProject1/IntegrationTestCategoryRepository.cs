using Xunit;
using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestProject1; // השם לפי המקום של DatabaseFixture

namespace TestProject
{
    public class IntegrationTestCategoryRepository : IClassFixture<DatabaseFixture>
    {
        private readonly MyShop_327882650Context _dbContext;
        private readonly CategoryRepository _repo;

        public IntegrationTestCategoryRepository(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context;
            _repo = new CategoryRepository(_dbContext);
        }

        [Fact]
        public async Task GetCategories_WhenCategoriesExist_ReturnsCategoriesWithProducts()
        {
            // Arrange
            var cat = new Category { CategoryName = "Books" };
            await _dbContext.Categories.AddAsync(cat);

            var prod = new Product { ProductName = "C# Book", Category = cat, Price = 100, Description = "Programming", ImageUrl = "cs.jpg" };
            await _dbContext.Products.AddAsync(prod);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repo.GetCategories();

            // Assert
            Assert.Single(result);
            Assert.Equal("Books", result[0].CategoryName);
            Assert.Single(result[0].Products);
            Assert.Equal("C# Book", result[0].Products.First().ProductName);
        }

        [Fact]
        public async Task GetCategories_WhenNoCategories_ReturnsEmptyList()
        {
            // Arrange
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repo.GetCategories();

            // Assert
            Assert.Empty(result);
        }
    }
}