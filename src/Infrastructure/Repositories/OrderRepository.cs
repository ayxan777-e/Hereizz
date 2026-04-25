using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository : GenericRepository<Order, int>, IOrderRepository
{
    private readonly HereizzzDbContext _context;

    public OrderRepository(HereizzzDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllWithDetailsAsync(CancellationToken ct)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id, CancellationToken ct)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public IQueryable<Order> GetQueryableWithDetails()
    {
        return _context.Orders
            .Include(x => x.Items)
            .AsQueryable();
    }

    public async Task<Order?> GetByIdWithUserAsync(int id, CancellationToken ct)
    {
        return await _context.Orders
            .Include(x => x.User)
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    public IQueryable<Order> GetQueryable()
    {
        return _context.Orders.AsQueryable();
    }
}