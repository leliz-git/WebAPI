using AutoMapper;
using DTO;
using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;

        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> GetProducts(string? desc, int? minprice, int? maxprice, int?[] categoriesId)
        {
           var products= await _productRepository.GetProducts(desc, minprice, maxprice, categoriesId);
            return products.Select(x=>_mapper.Map<ProductDTO>(x)).ToList();
        }
      
    }
}
