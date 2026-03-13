using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order, int>
{
}