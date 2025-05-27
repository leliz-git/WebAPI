using DTO;
using Entities;

namespace Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetProducts(string? desc, int? minprice, int? maxprice, int?[] categoriesId);
    }
}