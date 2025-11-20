using PriceService.Application.Interfaces;
using PriceService.Domain.Entities;

namespace PriceService.Application;

public class PriceService
{
    private readonly IEnumerable<IPriceProvider> _providers;
    private readonly IPriceAggregator _aggregator;
    private readonly IPriceRepository _repository;

    public PriceService(IEnumerable<IPriceProvider> providers, IPriceAggregator aggregator, IPriceRepository repository)
    {
        _providers = providers;
        _aggregator = aggregator;
        _repository = repository;
    }

    public async Task<object> GetAggregatedPriceAsync(DateTime utcHour)
    {
        var existing = await _repository.GetByHourAsync(utcHour);
        if (existing != null)
            return new { timestamp = utcHour, price = existing.AggregatedPrice, sourceCount = 0, isCached = true };

        var tasks = _providers.Select(async p => (p.Name, Price: await p.GetClosePriceAsync(utcHour)));
        var results = await Task.WhenAll(tasks);

        var valid = results.Where(x => x.Price != null).Select(x => x.Price!.Value).ToList();
        if (!valid.Any())
            throw new Exception("No valid provider data");

        var agg = _aggregator.Aggregate(valid);

        var rec = new PriceRecord { TimestampHour = utcHour, AggregatedPrice = agg, CreatedAt = DateTime.UtcNow };
        await _repository.SaveAsync(rec);

        return new { timestamp = utcHour, price = agg, sourceCount = valid.Count, isCached = false };
    }
}
