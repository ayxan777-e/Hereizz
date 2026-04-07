using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Shared.Responses;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IShippingOptionRepository _shippingOptionRepository;
    private readonly IPriceCalculatorService _priceCalculatorService;
    private readonly ILogger<CartService> _logger;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IShippingOptionRepository shippingOptionRepository,
        IPriceCalculatorService priceCalculatorService,
        ILogger<CartService> logger)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _shippingOptionRepository = shippingOptionRepository;
        _priceCalculatorService = priceCalculatorService;
        _logger = logger;
    }

    public async Task<BaseResponse> AddItemAsync(string userId, int productId, int shippingOptionId, int quantity, CancellationToken ct)
    {
        _logger.LogInformation(
            "Add to cart requested. UserId={UserId}, ProductId={ProductId}, ShippingOptionId={ShippingOptionId}, Quantity={Quantity}",
            userId,
            productId,
            shippingOptionId,
            quantity);

        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product is null)
        {
            _logger.LogWarning("Add to cart failed. Product not found. UserId={UserId}, ProductId={ProductId}", userId, productId);
            return BaseResponse.Fail(
                "Product not found",
                new List<string> { "The selected product does not exist." },
                ErrorType.NotFound);
        }

        var shippingOption = await _shippingOptionRepository.GetByIdAsync(shippingOptionId, ct);
        if (shippingOption is null)
        {
            _logger.LogWarning("Add to cart failed. Shipping option not found. UserId={UserId}, ShippingOptionId={ShippingOptionId}", userId, shippingOptionId);
            return BaseResponse.Fail(
                "Shipping option not found",
                new List<string> { "The selected shipping option does not exist." },
                ErrorType.NotFound);
        }

        var calculations = await _priceCalculatorService.CalculateAsync(productId, ct);
        var selectedCalculation = calculations.FirstOrDefault(x => x.ShippingOptionId == shippingOption.Id);

        if (selectedCalculation is null)
        {
            _logger.LogWarning(
                "Add to cart failed. Price calculation missing. UserId={UserId}, ProductId={ProductId}, ShippingOptionId={ShippingOptionId}",
                userId,
                productId,
                shippingOptionId);
            return BaseResponse.Fail(
                "Calculation failed",
                new List<string> { "Could not calculate the selected shipping option." },
                ErrorType.BadRequest);
        }

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);

        if (cart is null)
        {
            cart = new Cart { UserId = userId };
            await _cartRepository.AddAsync(cart, ct);
            await _cartRepository.SaveChangesAsync(ct);

            _logger.LogInformation("Cart created for UserId={UserId}, CartId={CartId}", userId, cart.Id);
        }

        var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == productId && x.ShippingOptionId == shippingOptionId);
        if (existingItem is not null)
        {
            existingItem.Quantity += quantity;
            _logger.LogInformation(
                "Cart item quantity increased. UserId={UserId}, CartItemId={CartItemId}, NewQuantity={NewQuantity}",
                userId,
                existingItem.Id,
                existingItem.Quantity);
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = quantity,
                ShippingOptionId = shippingOption.Id,
                UnitPrice = selectedCalculation.ProductPrice,
                ShippingCost = selectedCalculation.ShippingCost,
                CustomsFee = selectedCalculation.CustomsFee,
                WarehouseFee = selectedCalculation.WarehouseFee,
                LocalDeliveryFee = selectedCalculation.LocalDeliveryFee,
                FinalPrice = selectedCalculation.FinalPrice,
                ShippingOptionName = selectedCalculation.ShippingOptionName,
                TransportType = selectedCalculation.TransportType,
                EstimatedMinDays = selectedCalculation.EstimatedMinDays,
                EstimatedMaxDays = selectedCalculation.EstimatedMaxDays
            });

            _logger.LogInformation(
                "Cart item added. UserId={UserId}, CartId={CartId}, ProductId={ProductId}, ShippingOptionId={ShippingOptionId}",
                userId,
                cart.Id,
                productId,
                shippingOptionId);
        }

        await _cartRepository.SaveChangesAsync(ct);

        return BaseResponse.Ok("Item added to cart successfully");
    }

    public async Task<BaseResponse> RemoveItemAsync(string userId, int itemId, CancellationToken ct)
    {
        _logger.LogInformation("Remove cart item requested. UserId={UserId}, ItemId={ItemId}", userId, itemId);

        var cart = await _cartRepository.GetByUserIdWithItemsAsync(userId, ct);
        if (cart is null)
        {
            _logger.LogWarning("Remove cart item failed. Cart not found. UserId={UserId}", userId);
            return BaseResponse.Fail(
                "Cart not found",
                new List<string> { "Cart was not found for this user." },
                ErrorType.NotFound);
        }

        var cartItem = cart.Items.FirstOrDefault(x => x.Id == itemId);
        if (cartItem is null)
        {
            _logger.LogWarning("Remove cart item failed. Item not found. UserId={UserId}, ItemId={ItemId}", userId, itemId);
            return BaseResponse.Fail(
                "Cart item not found",
                new List<string> { "The requested cart item does not exist in your cart." },
                ErrorType.NotFound);
        }

        cart.Items.Remove(cartItem);
        await _cartRepository.SaveChangesAsync(ct);

        _logger.LogInformation("Cart item removed. UserId={UserId}, ItemId={ItemId}", userId, itemId);

        return BaseResponse.Ok("Cart item removed successfully.");
    }
}
