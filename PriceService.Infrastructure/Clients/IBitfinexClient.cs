namespace PriceService.Infrastructure.Clients;
public interface IBitfinexClient 
{ 
Task<double?> GetClosePriceAsync(DateTime utcHour); 
}