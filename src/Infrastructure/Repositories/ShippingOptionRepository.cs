using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ShippingOptionRepository
    : GenericRepository<ShippingOption, int>, IShippingOptionRepository
{
    private readonly HereizzzDbContext _context;

    public ShippingOptionRepository(HereizzzDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ShippingOption>> GetByOriginCountryAsync(
        Country originCountry,
        CancellationToken ct = default)
    {
        return await _context.ShippingOptions
            .Where(x => x.OriginCountry == originCountry && x.IsActive)
            .ToListAsync(ct);
    }
}