using Application.Commands.Product.DeleteProduct;
using FluentValidation;

namespace Application.Validators.Product;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}