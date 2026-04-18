using Application.DTOs.Product;
using Application.DTOs.ShippingOption;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductListItemResponse>();
        CreateMap<Product, ProductDetailResponse>();
        CreateMap<ShippingOption, ShippingOptionListItemResponse>();
        CreateMap<ShippingOption, ShippingOptionDetailResponse>();
    }
}