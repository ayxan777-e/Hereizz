using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces.Repositories;

public interface IShippingOptionRepository : IGenericRepository<ShippingOption, int>
{
    Task<List<ShippingOption>> GetByOriginCountryAsync(Country originCountry, CancellationToken ct = default);
}