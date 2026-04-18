using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product, int>
{
    Task<bool> ExistsByMarketplaceAndExternalProductIdAsync( Marketplace marketplace, string externalProductId,CancellationToken ct = default);

    Task<bool> ExistsByAffiliateUrlAsync(string affiliateUrl, int excludeId, CancellationToken ct = default);

    Task<List<Product>> GetAllActiveAsync(CancellationToken ct = default);
    Task<Product?> GetActiveByIdAsync(int id, CancellationToken ct = default);
}