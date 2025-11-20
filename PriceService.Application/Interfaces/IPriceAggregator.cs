namespace PriceService.Application.Interfaces;

public interface IPriceAggregator
{
    double Aggregate(IEnumerable<double> prices);
}
