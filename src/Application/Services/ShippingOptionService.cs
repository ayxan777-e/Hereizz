using Application.DTOs.ShippingOption;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ShippingOptionService : IShippingOptionService
{
    private readonly IShippingOptionRepository _shippingOptionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ShippingOptionService> _logger;

    public ShippingOptionService(
        IShippingOptionRepository shippingOptionRepository,
        IMapper mapper,
        ILogger<ShippingOptionService> logger)
    {
        _shippingOptionRepository = shippingOptionRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BaseResponse<List<ShippingOptionListItemResponse>>> GetAllAsync(bool adminView, CancellationToken ct)
    {
        _logger.LogInformation("Getting shipping option list. AdminView={AdminView}", adminView);

        var items = adminView
            ? await _shippingOptionRepository.GetAllAsync(ct)
            : await _shippingOptionRepository.GetAllActiveAsync(ct);

        var response = _mapper.Map<List<ShippingOptionListItemResponse>>(items);

        return BaseResponse<List<ShippingOptionListItemResponse>>.Ok(
            response,
            "Shipping option list uğurla gətirildi.");
    }

    public async Task<BaseResponse<ShippingOptionDetailResponse>> GetByIdAsync(int id, bool adminView, CancellationToken ct)
    {
        _logger.LogInformation("Getting shipping option detail. Id={Id}, AdminView={AdminView}", id, adminView);

        var entity = adminView
            ? await _shippingOptionRepository.GetByIdAsync(id, ct)
            : await _shippingOptionRepository.GetActiveByIdAsync(id, ct);

        if (entity is null)
        {
            return BaseResponse<ShippingOptionDetailResponse>.Fail(
                "Shipping option tapılmadı.",
                new List<string> { $"Id-si {id} olan uyğun shipping option mövcud deyil." },
                ErrorType.NotFound);
        }

        var response = _mapper.Map<ShippingOptionDetailResponse>(entity);

        return BaseResponse<ShippingOptionDetailResponse>.Ok(
            response,
            "Shipping option detail uğurla gətirildi.");
    }

    public async Task<BaseResponse<int>> CreateAsync(CreateShippingOptionRequest request, CancellationToken ct)
    {
        _logger.LogInformation(
            "Creating shipping option. Name={Name}, Origin={Origin}, Destination={Destination}, Transport={Transport}",
            request.Name,
            request.OriginCountry,
            request.DestinationCountry,
            request.TransportType);

        var exists = await _shippingOptionRepository.ExistsAsync(
            request.Name,
            request.OriginCountry,
            request.DestinationCountry,
            request.TransportType,
            ct);

        if (exists)
        {
            return BaseResponse<int>.Fail(
                "Bu route və transport üçün eyni adlı shipping option artıq mövcuddur.",
                new List<string> { "Duplicate shipping option." },
                ErrorType.Conflict);
        }

        var entity = new Domain.Entities.ShippingOption
        {
            Name = request.Name,
            OriginCountry = request.OriginCountry,
            DestinationCountry = request.DestinationCountry,
            TransportType = request.TransportType,
            EstimatedMinDays = request.EstimatedMinDays,
            EstimatedMaxDays = request.EstimatedMaxDays,
            PricePerKg = request.PricePerKg,
            FixedFee = request.FixedFee,
            IsActive = request.IsActive
        };

        await _shippingOptionRepository.AddAsync(entity, ct);
        await _shippingOptionRepository.SaveChangesAsync(ct);

        return BaseResponse<int>.Ok(entity.Id, "Shipping option uğurla yaradıldı.");
    }

    public async Task<BaseResponse> UpdateAsync(int id, UpdateShippingOptionRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Updating shipping option. Id={Id}", id);

        var entity = await _shippingOptionRepository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return BaseResponse.Fail(
                "Shipping option tapılmadı.",
                new List<string> { $"Id-si {id} olan shipping option mövcud deyil." },
                ErrorType.NotFound);
        }

        var exists = await _shippingOptionRepository.ExistsAsync(
            request.Name,
            request.OriginCountry,
            request.DestinationCountry,
            request.TransportType,
            id,
            ct);

        if (exists)
        {
            return BaseResponse.Fail(
                "Bu route və transport üçün eyni adlı başqa shipping option mövcuddur.",
                new List<string> { "Duplicate shipping option." },
                ErrorType.Conflict);
        }

        entity.Name = request.Name;
        entity.OriginCountry = request.OriginCountry;
        entity.DestinationCountry = request.DestinationCountry;
        entity.TransportType = request.TransportType;
        entity.EstimatedMinDays = request.EstimatedMinDays;
        entity.EstimatedMaxDays = request.EstimatedMaxDays;
        entity.PricePerKg = request.PricePerKg;
        entity.FixedFee = request.FixedFee;
        entity.IsActive = request.IsActive;

        _shippingOptionRepository.Update(entity);
        await _shippingOptionRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Shipping option uğurla yeniləndi.");
    }

    public async Task<BaseResponse> DeactivateAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Deactivating shipping option. Id={Id}", id);

        var entity = await _shippingOptionRepository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return BaseResponse.Fail(
                "Shipping option tapılmadı.",
                new List<string> { $"Id-si {id} olan shipping option mövcud deyil." },
                ErrorType.NotFound);
        }

        if (!entity.IsActive)
        {
            return BaseResponse.Fail(
                "Shipping option artıq deaktivdir.",
                new List<string> { "Shipping option is already inactive." },
                ErrorType.BusinessRule);
        }

        entity.IsActive = false;

        _shippingOptionRepository.Update(entity);
        await _shippingOptionRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Shipping option deaktiv edildi.");
    }

    public async Task<BaseResponse> ActivateAsync(int id, CancellationToken ct)
    {
        _logger.LogInformation("Activating shipping option. Id={Id}", id);

        var entity = await _shippingOptionRepository.GetByIdAsync(id, ct);
        if (entity is null)
        {
            return BaseResponse.Fail(
                "Shipping option tapılmadı.",
                new List<string> { $"Id-si {id} olan shipping option mövcud deyil." },
                ErrorType.NotFound);
        }

        if (entity.IsActive)
        {
            return BaseResponse.Fail(
                "Shipping option artıq aktivdir.",
                new List<string> { "Shipping option is already active." },
                ErrorType.BusinessRule);
        }

        entity.IsActive = true;

        _shippingOptionRepository.Update(entity);
        await _shippingOptionRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Shipping option aktiv edildi.");
    }
}