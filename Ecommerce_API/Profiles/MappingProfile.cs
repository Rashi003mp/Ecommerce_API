using AutoMapper;
using Ecommerce_API.DTOs.CategoryDTO;
using Ecommerce_API.DTOs.ProductDTO;
using Ecommerce_API.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDTO>();

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.CategoryName,
                       opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.ImageUrls,
                       opt => opt.MapFrom(src => src.Images
                           .Select(i => i.ImageUrl)
                           .ToList()
                       ));

        CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.Images, opt => opt.Ignore());

        CreateMap<UpdateProductDTO, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));
    }
}
