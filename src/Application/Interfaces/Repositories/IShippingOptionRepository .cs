using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IShippingOptionRepository : IGenericRepository<ShippingOption, int>
{
    Task<List<ShippingOption>> GetByOriginCountryAsync(Country originCountry, CancellationToken ct = default);

    Task<bool> ExistsAsync(
        string name,
        Country originCountry,
        Country destinationCountry,
        TransportType transportType,
        CancellationToken ct = default);

    Task<bool> ExistsAsync(
        string name,
        Country originCountry,
        Country destinationCountry,
        TransportType transportType,
        int excludeId,
        CancellationToken ct = default);

    Task<List<ShippingOption>> GetAllActiveAsync(CancellationToken ct = default);
    Task<ShippingOption?> GetActiveByIdAsync(int id, CancellationToken ct = default);
}