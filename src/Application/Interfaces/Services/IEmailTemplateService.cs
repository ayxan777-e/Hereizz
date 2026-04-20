using Domain.Enums;

namespace Application.Interfaces.Services;

public interface IEmailTemplateService
{
    string BuildConfirmEmailTemplate(string confirmationLink);

    string BuildOrderCreatedTemplate(string productsText, OrderStatus status);

    (string Subject, string Body) BuildOrderStatusUpdatedTemplate(string productsText, OrderStatus status);
}