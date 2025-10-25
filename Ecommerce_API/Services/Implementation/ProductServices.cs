using AutoMapper;
using Ecommerce_API.Common;
using Ecommerce_API.DTOs.ProductDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Implementation;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;

namespace Ecommerce_API.Services.Implementation
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IGenericRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductServices(IMapper mapper, IGenericRepository<Product> repository, IProductRepository productRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _productRepo = productRepository;
        }

        public async Task<ApiResponse<ProductDTO?>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepo.GetProductWithDetailsAsync(id);

                if (product == null)
                    return new ApiResponse<ProductDTO?>(404, $"Product not found with id: {id}");

                var result = _mapper.Map<ProductDTO>(product);

                return new ApiResponse<ProductDTO?>(200, "Product Retrived Successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ProductDTO?>(500, $"Error fetching product: {ex.Message}");
            }
        }


    }
}
