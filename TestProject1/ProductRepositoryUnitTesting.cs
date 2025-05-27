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
    public class ProductRepositoryUnitTesting
    {
        [Fact]
        public async Task GetProducts_FiltersByDescriptionAndPriceAndCategory()
        {
            // Arrange
            var cat1 = new Category { CategoryId = 1, CategoryName = "Books" };
            var cat2 = new Category { CategoryId = 2, CategoryName = "Toys" };
            var products = new List<Product>
            {
                new Product { ProductId = 1, Description = "Harry Potter", Price = 50, CategoryId = 1, Category = cat1 },
                new Product { ProductId = 2, Description = "LEGO City", Price = 120, CategoryId = 2, Category = cat2 },
                new Product { ProductId = 3, Description = "Notebook", Price = 20, CategoryId = 1, Category = cat1 }
            };

            var mockContext = new Mock<MyShop_327882650Context>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);

            string desc = "Harry";
            int? minPrice = 40;
            int? maxPrice = 60;
            int?[] categories = new int?[] { 1 };

            // Act
            var result = await repository.GetProducts(desc, minPrice, maxPrice, categories);

            // Assert
            //Assert.Single(result);
            Assert.Equal("Harry Potter", result[0].Description);
            Assert.Equal(50, result[0].Price);
            Assert.Equal(1, result[0].CategoryId);
        }

        [Fact]
        public async Task GetProducts_ReturnsAll_WhenNoFilter()
        {
            // Arrange
            var cat1 = new Category { CategoryId = 1, CategoryName = "Books" };
            var cat2 = new Category { CategoryId = 2, CategoryName = "Toys" };
            var products = new List<Product>
            {
                new Product { ProductId = 1, Description = "Harry Potter", Price = 50, CategoryId = 1, Category = cat1 },
                new Product { ProductId = 2, Description = "LEGO City", Price = 120, CategoryId = 2, Category = cat2 },
                new Product { ProductId = 3, Description = "Notebook", Price = 20, CategoryId = 1, Category = cat1 }
            };

            var mockContext = new Mock<MyShop_327882650Context>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);

            // Act
            var result = await repository.GetProducts(null, null, null, Array.Empty<int?>());

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetProducts_FiltersByCategory()
        {
            // Arrange
            var cat1 = new Category { CategoryId = 1, CategoryName = "Books" };
            var cat2 = new Category { CategoryId = 2, CategoryName = "Toys" };
            var products = new List<Product>
            {
                new Product { ProductId = 1, Description = "Harry Potter", Price = 50, CategoryId = 1, Category = cat1 },
                new Product { ProductId = 2, Description = "LEGO City", Price = 120, CategoryId = 2, Category = cat2 },
                new Product { ProductId = 3, Description = "Notebook", Price = 20, CategoryId = 1, Category = cat1 }
            };

            var mockContext = new Mock<MyShop_327882650Context>();
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);

            var repository = new ProductRepository(mockContext.Object);
            int?[] categories = new int?[] { 2 };

            // Act
            var result = await repository.GetProducts(null, null, null, categories);

            // Assert
            //Assert.Single(result);
            Assert.Equal("LEGO City", result[0].Description);
        }
    }
}

