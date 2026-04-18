using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.UpdateProduct;

public class UpdateProductCommandHandler
    : IRequestHandler<UpdateProductCommand, BaseResponse>
{
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BaseResponse> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        return await _productService.UpdateAsync(
            request.Id,
            request.Request,
            cancellationToken);
    }
}