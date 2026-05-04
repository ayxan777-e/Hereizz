using Application.Interfaces.Services;

public class CurrencyService : ICurrencyService
{
    public decimal ConvertToAzn(decimal amount, string currency)
    {
        if (currency == "AZN")
            return amount;

        if (currency == "USD")
            return amount * 1.7m;

        if (currency == "EUR")
            return amount * 1.85m;

        return amount;
    }
}