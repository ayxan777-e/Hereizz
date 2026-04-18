using Application.DTOs.Product;
using Application.Shared.Responses;
using MediatR;

namespace Application.Commands.Product.ImportProduct;

public class ImportProductCommand : IRequest<BaseResponse<int>>
{
    public ImportProductRequest Request { get; set; }

    public ImportProductCommand(ImportProductRequest request)
    {
        Request = request;
    }
}