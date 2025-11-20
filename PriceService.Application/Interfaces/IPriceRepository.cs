using PriceService.Domain.Entities;

namespace PriceService.Application.Interfaces;

public interface IPriceRepository
{
    Task<PriceRecord?> GetByHourAsync(DateTime utcHour);
    Task SaveAsync(PriceRecord rec);
    Task<IEnumerable<PriceRecord>> GetRangeAsync(DateTime startUtc, DateTime endUtc);
}
