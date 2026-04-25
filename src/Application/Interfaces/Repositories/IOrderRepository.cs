using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order, int>
{
    Task<List<Order>> GetAllWithDetailsAsync(CancellationToken ct);
    Task<Order?> GetByIdWithDetailsAsync(int id, CancellationToken ct);
    IQueryable<Order> GetQueryableWithDetails();
    Task<Order?> GetByIdWithUserAsync(int id, CancellationToken ct);
    IQueryable<Order> GetQueryable();
}
