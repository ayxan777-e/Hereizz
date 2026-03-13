using Application.DTOs.Orders;
using Application.Shared.Responses;
using MediatR;

namespace Application.Queries.Orders.GetOrderById;

public class GetOrderByIdQuery : IRequest<BaseResponse<OrderDetailsDto>>
{
    public int Id { get; set; }

    public GetOrderByIdQuery(int id)
    {
        Id = id;
    }
}