using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product, int>, IProductRepository
{
    public ProductRepository(HereizzzDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByMarketplaceAndExternalProductIdAsync(
        Marketplace marketplace,
        string externalProductId,
        CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(
            x => x.Marketplace == marketplace && x.ExternalProductId == externalProductId,
            ct);
    }

    public async Task<bool> ExistsByAffiliateUrlAsync(string affiliateUrl, int excludeId, CancellationToken ct = default)
    {
        return await _dbSet.AnyAsync(x => x.AffiliateUrl == affiliateUrl && x.Id != excludeId, ct);
    }

    public async Task<List<Product>> GetAllActiveAsync(CancellationToken ct = default)
    {
        return await _dbSet
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }

    public async Task<Product?> GetActiveByIdAsync(int id, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    }
}