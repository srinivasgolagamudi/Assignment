namespace PriceService.Application.Interfaces;

public interface IPriceProvider
{
    Task<double?> GetClosePriceAsync(DateTime utcHour);
    string Name { get; }
}
