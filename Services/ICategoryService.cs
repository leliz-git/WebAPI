using DTO;
using Entities;

namespace Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
    }
}