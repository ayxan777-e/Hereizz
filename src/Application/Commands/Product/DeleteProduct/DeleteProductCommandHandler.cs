using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.DeleteProduct;

public class DeleteProductCommandHandler
    : IRequestHandler<DeleteProductCommand, BaseResponse>
{
    private readonly IProductService _productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BaseResponse> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        return await _productService.DeactivateAsync(
            request.Id,
            cancellationToken);
    }
}