using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Payment, int>, IPaymentRepository
{
    public PaymentRepository(HereizzzDbContext context) : base(context)
    {
    }

    public async Task<List<Payment>> GetByUserIdAsync(string userId, CancellationToken ct)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public IQueryable<Payment> GetQueryable()
    {
        return _context.Payments.AsQueryable();
    }
}