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

    public async Task<bool> ExistsAsync(
        string name,
        Country originCountry,
        Country destinationCountry,
        TransportType transportType,
        CancellationToken ct = default)
    {
        return await _context.ShippingOptions.AnyAsync(
            x => x.Name == name &&
                 x.OriginCountry == originCountry &&
                 x.DestinationCountry == destinationCountry &&
                 x.TransportType == transportType,
            ct);
    }

    public async Task<bool> ExistsAsync(
        string name,
        Country originCountry,
        Country destinationCountry,
        TransportType transportType,
        int excludeId,
        CancellationToken ct = default)
    {
        return await _context.ShippingOptions.AnyAsync(
            x => x.Name == name &&
                 x.OriginCountry == originCountry &&
                 x.DestinationCountry == destinationCountry &&
                 x.TransportType == transportType &&
                 x.Id != excludeId,
            ct);
    }

    public async Task<List<ShippingOption>> GetAllActiveAsync(CancellationToken ct = default)
    {
        return await _context.ShippingOptions
            .Where(x => x.IsActive)
            .ToListAsync(ct);
    }

    public async Task<ShippingOption?> GetActiveByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.ShippingOptions
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    }
}