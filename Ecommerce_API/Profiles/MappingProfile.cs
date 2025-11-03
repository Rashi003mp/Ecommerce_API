using AutoMapper;
using Ecommerce_API.DTOs.AddressDTO;
using Ecommerce_API.DTOs.CategoryDTO;
using Ecommerce_API.DTOs.OrderDTO;
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

        CreateMap<AddressDto, Address>()
    .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Address, AddressDto>();

        CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Product.Images.Select(i => i.ImageUrl).ToList()));

        // Order Mappings
        CreateMap<Order, OrderResponseDTO>()
            .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))
            .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ReverseMap();

        // Create Order DTO → Order Entity
        CreateMap<CreateOrderDTO, Order>()
            .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.AddressId.HasValue ? src.AddressId.Value : 0))
            .ForMember(dest => dest.Address, opt => opt.Ignore())  // we'll handle address manually
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());


    }
}
