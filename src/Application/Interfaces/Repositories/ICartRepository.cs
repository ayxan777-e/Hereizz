using Domain.Entities;

namespace Application.Interfaces.Repositories;


public interface ICartRepository
{
    Task<Cart?> GetByUserIdWithItemsAsync(string userId, CancellationToken ct = default);
    Task AddAsync(Cart cart, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task ClearCartAsync(int cartId, CancellationToken ct);
}

