using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment, int>
{
    Task<List<Payment>> GetByUserIdAsync(string userId, CancellationToken ct);
    IQueryable<Payment> GetQueryable();
}