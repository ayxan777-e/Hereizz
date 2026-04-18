using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductListItemResponse>();
        CreateMap<Product, ProductDetailResponse>();
    }
}