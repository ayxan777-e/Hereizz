using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.ActivateProduct;

public class ActivateProductCommandHandler
    : IRequestHandler<ActivateProductCommand, BaseResponse>
{
    private readonly IProductService _productService;

    public ActivateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BaseResponse> Handle(
        ActivateProductCommand request,
        CancellationToken cancellationToken)
    {
        return await _productService.ActivateAsync(
            request.Id,
            cancellationToken);
    }
}