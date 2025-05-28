using Xunit;
using Entities;
using Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestProject1;

namespace TestProject
{
    public class IntegrationTestProductRepository : IClassFixture<DatabaseFixture>
    {
        private readonly MyShop_327882650Context _dbContext;
        private readonly ProductRepository _repo;

        public IntegrationTestProductRepository(DatabaseFixture fixture)
        {
            _dbContext = fixture.Context; // שימוש ב-Context מה-fixture
            _repo = new ProductRepository(_dbContext);
        }

        private async Task SeedProductsAsync()
        {
            _dbContext.Products.RemoveRange(_dbContext.Products);
            _dbContext.Categories.RemoveRange(_dbContext.Categories);
            await _dbContext.SaveChangesAsync();

            var cat1 = new Category {};
            var cat2 = new Category {};
            await _dbContext.Categories.AddRangeAsync(cat1, cat2);
            await _dbContext.SaveChangesAsync();

            var p1 = new Product { ProductName = "iPhone", Description = "Apple smartphone", Price = 4000, CategoryId = cat1.CategoryId, ImageUrl = "iphone.jpg" };
            var p2 = new Product { ProductName = "Samsung TV", Description = "Smart TV", Price = 3500, CategoryId = cat1.CategoryId, ImageUrl = "tv.jpg" };
            var p3 = new Product { ProductName = "C# Book", Description = "Programming Book", Price = 180, CategoryId = cat2.CategoryId, ImageUrl = "cs.jpg" };
            var p4 = new Product { ProductName = "Novel", Description = "Literature", Price = 90, CategoryId = cat2.CategoryId, ImageUrl = "novel.jpg" };
            await _dbContext.Products.AddRangeAsync(p1, p2, p3, p4);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GetProducts_NoFilters_ReturnsAllOrderedByPrice()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts(null, null, null, new int?[0]);
            Assert.Equal(4, result.Count);
            Assert.True(result.SequenceEqual(result.OrderBy(p => p.Price)));
        }

        [Fact]
        public async Task GetProducts_FilterByProductName()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts("Book", null, null, new int?[0]);
            Assert.Single(result);
            Assert.Contains(result, p => p.ProductName.Contains("Book"));
        }

        [Fact]
        public async Task GetProducts_FilterByDescription()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts("Programming", null, null, new int?[0]);
            Assert.Single(result);
            Assert.Contains(result, p => p.Description.Contains("Programming"));
        }

        [Fact]
        public async Task GetProducts_FilterByMinPrice()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts(null, 1000, null, new int?[0]);
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.True(p.Price >= 1000));
        }

        [Fact]
        public async Task GetProducts_FilterByMaxPrice()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts(null, null, 200, new int?[0]);
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.True(p.Price <= 200));
        }

        [Fact]
        public async Task GetProducts_FilterByCategory()
        {
            await SeedProductsAsync();
            var catId = 1;

            var result = await _repo.GetProducts(null, null, null, new int?[] { catId });
            Assert.Equal(2, result.Count);
            Assert.All(result, p => Assert.Equal(catId, p.CategoryId));
        }

        [Fact]
        public async Task GetProducts_FilterByMultipleCategories()
        {
            await SeedProductsAsync();
            var catIds = new int?[] { 1, 2 };

            var result = await _repo.GetProducts(null, null, null, catIds);
            Assert.Equal(4, result.Count);
        }

        [Fact]

        public async Task GetProducts_AllFiltersCombined()
        {
            await SeedProductsAsync();
            var catId = 2;

            // מחפש לפי Description ("Literature") ולא שם מוצר
            var result = await _repo.GetProducts("Literature", 50, 150, new int?[] { catId });
            Assert.Single(result);
            Assert.Equal("Novel", result[0].ProductName);
            Assert.Equal(catId, result[0].CategoryId);
        }

        [Fact]
        public async Task GetProducts_EmptyResult()
        {
            await SeedProductsAsync();

            var result = await _repo.GetProducts("NonExisting", null, null, new int?[0]);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProducts_NoMatchingFilters_ReturnsEmptyList()
        {
            // Arrange
            var category = new Category { CategoryName = "CatX" };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            var product = new Product
            {
                ProductName = "X",
                Description = "Desc",
                Price = 10,
                CategoryId = category.CategoryId
            };
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            var repo = new ProductRepository(_dbContext);

            // Act
            var result = await repo.GetProducts(
                "NotExist",         // מחרוזת שלא קיימת בשם או בתיאור
                100,                // מחיר מינימום גבוה מהמוצר
                200,                // מחיר מקסימום גבוה
                new int?[] { 999 }  // קטגוריה שלא קיימת
            );

            // Assert
            Assert.Empty(result);
        }
    }
}