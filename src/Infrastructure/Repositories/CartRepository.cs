using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly HereizzzDbContext _context;

    public CartRepository(HereizzzDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdWithItemsAsync(string userId, CancellationToken ct = default)
    {
        return await _context.Carts
            .Include(x => x.Items)
                .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);
    }

    public async Task AddAsync(Cart cart, CancellationToken ct = default)
    {
        await _context.Carts.AddAsync(cart, ct);
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}