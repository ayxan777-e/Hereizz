namespace Application.Interfaces.Services;

public interface ICurrencyService
{
    decimal ConvertToAzn(decimal amount, string currency);
}
