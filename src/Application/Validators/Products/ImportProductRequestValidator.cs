using Application.Commands.Product.ImportProduct;
using FluentValidation;

namespace Application.Validators.Product;

public class ImportProductCommandValidator : AbstractValidator<ImportProductCommand>
{
    public ImportProductCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request boş ola bilməz.");

        RuleFor(x => x.Request.ExternalProductId)
            .NotEmpty()
            .WithMessage("ExternalProductId boş ola bilməz.")
            .MaximumLength(100);

        RuleFor(x => x.Request.Marketplace)
            .IsInEnum()
            .WithMessage("Marketplace düzgün deyil.");
    }
}