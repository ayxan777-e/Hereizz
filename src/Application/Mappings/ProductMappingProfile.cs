using Application.DTOs.Product;
using AutoMapper;
using Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Mappings;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductListItemResponse>();
        CreateMap<Product, ProductDetailResponse>();
    }
}