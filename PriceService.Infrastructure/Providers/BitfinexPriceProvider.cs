using PriceService.Application.Interfaces;
using PriceService.Infrastructure.Clients;

namespace PriceService.Infrastructure;

public class BitfinexPriceProvider : IPriceProvider
{
    private readonly IBitfinexClient _client;
    public string Name => "Bitfinex";
    public BitfinexPriceProvider(IBitfinexClient client) => _client = client;
    public Task<double?> GetClosePriceAsync(DateTime utcHour) => _client.GetClosePriceAsync(utcHour);
}
