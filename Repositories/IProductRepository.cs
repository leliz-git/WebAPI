using Entities;

namespace Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(string? desc, int? minprice, int? maxprice, int?[] categoriesId);
    }
}