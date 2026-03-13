using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
{
    public OrderRepository(HereizzzDbContext context) : base(context)
    {
    }
}