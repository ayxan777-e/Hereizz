using Application.Interfaces.Services;
using Domain.Enums;

namespace Application.Services;

public class EmailTemplateService : IEmailTemplateService
{
    public string BuildConfirmEmailTemplate(string confirmationLink)
    {
        return $"""
            <div style="font-family: Arial, sans-serif; padding: 20px;">
                <h2 style="color: #2c3e50;">Hereizzz</h2>

                <h3 style="color: #34495e;">Confirm your email</h3>

                <p style="font-size: 15px;">
                    Please confirm your email address by clicking the button below.
                </p>

                <p style="margin: 20px 0;">
                    <a href="{confirmationLink}" 
                       style="background-color: #2c3e50; color: white; padding: 10px 18px; text-decoration: none; border-radius: 6px;">
                        Confirm Email
                    </a>
                </p>

                <p style="font-size: 13px; color: gray;">
                    If you did not create this account, you can ignore this email.
                </p>

                <p style="font-size: 13px; color: gray;">
                    Best regards,<br/>
                    <b>Hereizzz Team</b>
                </p>
            </div>
            """;
    }

    public string BuildOrderCreatedTemplate(string productsText, OrderStatus status)
    {
        return $"""
            <div style="font-family: Arial, sans-serif; padding: 20px;">
                <h2 style="color: #2c3e50;">Hereizzz</h2>

                <h3 style="color: #34495e;">Your order has been created</h3>

                <p style="font-size: 15px;">
                    Your order for <b>{productsText}</b> has been created successfully.
                </p>

                <p style="font-size: 15px;">
                    Current Status: <b>{status}</b>
                </p>

                <hr style="margin:20px 0;" />

                <p style="font-size: 13px; color: gray;">
                    We will keep you updated about the next steps.
                </p>

                <p style="font-size: 13px; color: gray;">
                    Best regards,<br/>
                    <b>Hereizzz Team</b>
                </p>
            </div>
            """;
    }

    public (string Subject, string Body) BuildOrderStatusUpdatedTemplate(string productsText, OrderStatus status)
    {
        string subject;
        string content;

        switch (status)
        {
            case OrderStatus.Confirmed:
                subject = "Your order has been confirmed";
                content = $"Your order for <b>{productsText}</b> has been successfully confirmed.";
                break;

            case OrderStatus.Processing:
                subject = "Your order is being processed";
                content = $"We are currently preparing your order for <b>{productsText}</b>.";
                break;

            case OrderStatus.Shipped:
                subject = "Your order is on the way";
                content = $"Your order for <b>{productsText}</b> has been shipped and is on the way.";
                break;

            case OrderStatus.Delivered:
                subject = "Your order has been delivered";
                content = $"Your order for <b>{productsText}</b> has been delivered successfully.";
                break;

            case OrderStatus.Cancelled:
                subject = "Your order has been cancelled";
                content = $"Your order for <b>{productsText}</b> has been cancelled.";
                break;

            default:
                subject = "Order status updated";
                content = $"Your order status for <b>{productsText}</b> has been updated to <b>{status}</b>.";
                break;
        }

        var body = $"""
            <div style="font-family: Arial, sans-serif; padding: 20px;">
                <h2 style="color: #2c3e50;">Hereizzz</h2>
                
                <h3 style="color: #34495e;">{subject}</h3>
                
                <p style="font-size: 15px;">
                    {content}
                </p>

                <hr style="margin:20px 0;" />

                <p style="font-size: 13px; color: gray;">
                    If you have any questions, feel free to contact us.
                </p>

                <p style="font-size: 13px; color: gray;">
                    Best regards,<br/>
                    <b>Hereizzz Team</b>
                </p>
            </div>
            """;

        return (subject, body);
    }
}