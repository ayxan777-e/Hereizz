using Application.Interfaces.Services;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.ImportProduct;

public class ImportProductCommandHandler
    : IRequestHandler<ImportProductCommand, BaseResponse<int>>
{
    private readonly IProductService _productService;

    public ImportProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<BaseResponse<int>> Handle(
        ImportProductCommand request,
        CancellationToken cancellationToken)
    {
        return await _productService.ImportAsync(request.Request, cancellationToken);
    }
}