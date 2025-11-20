using PriceService.Application.Interfaces;
using PriceService.Infrastructure.Clients;

namespace PriceService.Infrastructure;

public class BitstampPriceProvider : IPriceProvider
{
    private readonly IBitstampClient _client;
    public string Name => "Bitstamp";
    public BitstampPriceProvider(IBitstampClient client) => _client = client;
    public Task<double?> GetClosePriceAsync(DateTime utcHour) => _client.GetClosePriceAsync(utcHour);
}
