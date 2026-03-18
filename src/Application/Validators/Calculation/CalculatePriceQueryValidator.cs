using FluentValidation;
using Application.Queries.Calculation.CalculatePrice;

namespace Application.Validations.Calculation;

public class CalculatePriceQueryValidator : AbstractValidator<CalculatePriceQuery>
{
    public CalculatePriceQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0");
    }
}