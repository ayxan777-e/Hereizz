using Application.Commands.Product.UpdateProduct;
using FluentValidation;

namespace Application.Validators.Product;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Request.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Request.Price)
            .GreaterThan(0);

        RuleFor(x => x.Request.WeightKg)
            .GreaterThan(0);

        RuleFor(x => x.Request.Category)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Request.AffiliateUrl)
            .NotEmpty()
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("AffiliateUrl düzgün formatda deyil.");

        RuleFor(x => x.Request.Currency)
            .IsInEnum();

        RuleFor(x => x.Request.OriginCountry)
            .IsInEnum();
    }
}