using FluentValidation;
using Application.Queries.Orders.GetAllOrders;

namespace Application.Validations.Orders;

public class GetAllOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetAllOrdersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("PageNumber must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("PageSize must be between 1 and 50");
    }
}