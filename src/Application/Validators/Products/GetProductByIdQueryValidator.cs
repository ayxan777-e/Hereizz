using FluentValidation;
using Application.Queries.Products.GetProductById;

namespace Application.Validations.Products;

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product id must be greater than 0");
    }
}