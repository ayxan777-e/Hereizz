using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositories;

public class ProductRepository:GenericRepository<Product,int>, IProductRepository
{
    public ProductRepository(HereizzzDbContext context) : base(context)
    {
    }
}
