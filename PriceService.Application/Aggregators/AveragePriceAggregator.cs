using PriceService.Application.Interfaces;

namespace PriceService.Application;

public class AveragePriceAggregator : IPriceAggregator
{
    public double Aggregate(IEnumerable<double> prices)
    {
        var arr = prices.ToArray();
        if (!arr.Any()) throw new Exception("No prices to aggregate");
        return arr.Average();
    }
}
