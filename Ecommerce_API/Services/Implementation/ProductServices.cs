using AutoMapper;
using Ecommerce_API.Common;
using Ecommerce_API.DTOs.ProductDTO;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Ecommerce_API.Services.Interfaces;
using System.Net;

namespace Ecommerce_API.Services.Implementation
{
    public class ProductServices : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IGenericRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public ProductServices(
            IMapper mapper,
            IGenericRepository<Product> repository,
            IProductRepository productRepository,
            ICloudinaryService cloudinaryService)   
        {
            _mapper = mapper;
            _repository = repository;
            _productRepo = productRepository;
            _cloudinaryService = cloudinaryService;
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

        public async Task<ApiResponse<ProductDTO>> AddProductAsync(CreateProductDTO model)
        {
            if (model == null)
                return new ApiResponse<ProductDTO>(400, "Invalid product data");

            if (model.Images == null || !model.Images.Any())
                return new ApiResponse<ProductDTO>(400, "At least one image is required.");

            if (model.MainImageIndex < 0 || model.MainImageIndex >= model.Images.Count)
                return new ApiResponse<ProductDTO>(400, "MainImageIndex is out of range.");

            var uploadedPublicIds = new List<string>();

            try
            {
                // Map DTO -> Entity (Images ignored by mapping as configured)
                var product = _mapper.Map<Product>(model);

                product.IsActive = true;
                product.InStock = model.CurrentStock > 0;
                product.Images = new List<ProductImage>();

                // Upload files to Cloudinary and add to product.Images
                for (int i = 0; i < model.Images.Count; i++)
                {
                    var file = model.Images[i];
                    var (url, publicId) = await _cloudinaryService.UploadImageAsync(file);

                    // track uploaded id for cleanup if needed
                    uploadedPublicIds.Add(publicId);

                    var img = new ProductImage
                    {
                        ImageUrl = url,
                        PublicId = publicId,
                        IsMain = (i == model.MainImageIndex)
                    };

                    product.Images.Add(img);
                }

                // Save product (and images) to DB in one go
                await _repository.AddAsync(product); // Generic repo saves changes

                // Fetch product with details to ensure Category and Images are loaded (optional)
                var created = await _productRepo.GetProductWithDetailsAsync(product.Id);

                var dto = _mapper.Map<ProductDTO>(created);

                return new ApiResponse<ProductDTO>(201, "Product created successfully", dto);
            }
            catch (Exception ex)
            {
                // Attempt cleanup: delete any images uploaded to Cloudinary
                foreach (var pid in uploadedPublicIds)
                {
                    try
                    {
                        await _cloudinaryService.DeleteImageAsync(pid);
                    }
                    catch
                    {
                        // swallow cleanup exceptions (log if you have logging)
                    }
                }

                return new ApiResponse<ProductDTO>(500, $"Error creating product: {ex.Message}");
            }
        }

        public async Task<ApiResponse<IEnumerable<ProductDTO>>> GetFilteredProducts(
                 string? name = null,
                 int? categoryId = null,
                 decimal? minPrice = null,
                 decimal? maxPrice = null,
                 bool? inStock = null,
                 int page = 1,
                 int pageSize = 20,
                 string? sortBy = null,
                 bool descending = false)
        {
            try
            {
                var products = await _productRepo.GetFilteredProductsAsync(
                    name, categoryId, minPrice, maxPrice, inStock, page, pageSize, sortBy, descending);

                var result = _mapper.Map<IEnumerable<ProductDTO>>(products);

                return new ApiResponse<IEnumerable<ProductDTO>>(200, "Products retrieved successfully", result);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ProductDTO>>(500, $"Error fetching products: {ex.Message}", null);
            }
        }
        public async Task<ApiResponse<string>> UpdateProductAsync(int id, UpdateProductDTO model)
        {
            try
            {
                var product = await _productRepo.GetProductWithDetailsAsync(id);
                if (product == null)
                    return new ApiResponse<string>(404, $"Product not found with id: {id}");

                // ✅ Update fields only if provided (clean logic)
                if (!string.IsNullOrWhiteSpace(model.Name)) product.Name = model.Name.Trim();
                if (!string.IsNullOrWhiteSpace(model.Description)) product.Description = model.Description.Trim();
                if (model.Price.HasValue) product.Price = model.Price.Value;
                if (model.CategoryId.HasValue) product.CategoryId = model.CategoryId.Value;
                if (model.CurrentStock.HasValue)
                {
                    product.CurrentStock = model.CurrentStock.Value;
                    product.InStock = model.CurrentStock.Value > 0; // auto update
                }
                if (model.IsActive.HasValue) product.IsActive = model.IsActive.Value;

                // ✅ IMAGE UPDATE LOGIC (Reused same logic but cleaner style)
                if (model.ReplaceImages && model.NewImages != null && model.NewImages.Any())
                {
                    // Remove all old images
                    foreach (var img in product.Images)
                        await _cloudinaryService.DeleteImageAsync(img.PublicId);

                    product.Images.Clear();

                    // Add new images
                    foreach (var file in model.NewImages)
                    {
                        var (url, publicId) = await _cloudinaryService.UploadImageAsync(file);
                        product.Images.Add(new ProductImage
                        {
                            ImageUrl = url,
                            PublicId = publicId,
                            IsMain = false
                        });
                    }

                    // Set first image as main
                    if (product.Images.Any())
                        product.Images.First().IsMain = true;
                }
                else
                {
                    // Delete selected images
                    if (model.ImagesToDelete != null && model.ImagesToDelete.Any())
                    {
                        foreach (var publicId in model.ImagesToDelete)
                        {
                            var image = product.Images.FirstOrDefault(i => i.PublicId == publicId);
                            if (image != null)
                            {
                                await _cloudinaryService.DeleteImageAsync(publicId);
                                product.Images.Remove(image);
                            }
                        }
                    }

                    // Add new ones
                    if (model.NewImages != null && model.NewImages.Any())
                    {
                        foreach (var file in model.NewImages)
                        {
                            var (url, publicId) = await _cloudinaryService.UploadImageAsync(file);
                            product.Images.Add(new ProductImage
                            {
                                ImageUrl = url,
                                PublicId = publicId,
                                IsMain = false
                            });
                        }
                    }
                }

                // ✅ Save changes
                await _repository.UpdateAsync(product);

                return new ApiResponse<string>(200, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(500, $"Error updating product: {ex.Message}");
            }
        }


    }
}
