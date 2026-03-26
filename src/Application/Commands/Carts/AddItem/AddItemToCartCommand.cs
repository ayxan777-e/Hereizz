using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Cart.AddItem;

public class AddItemToCartCommand : IRequest<BaseResponse>
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    // 🔥 Route seçimi
    public int ShippingOptionId { get; set; }
}