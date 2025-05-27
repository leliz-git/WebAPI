using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using AutoMapper;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper=mapper;
        }
        public async Task<List<CategoryDTO>> GetCategories()
        {

            var categories= await _categoryRepository.GetCategories();
            return categories.Select(x => _mapper.Map<CategoryDTO>(x)).ToList();
        }
    } 
}
