using Microsoft.EntityFrameworkCore;
using PriceService.Application.Interfaces;
using PriceService.Domain.Entities;
using PriceService.Infrastructure.Data;

namespace PriceService.Infrastructure.Repositories;

public class PriceRepository : IPriceRepository
{
    private readonly AppDbContext _db;

    public PriceRepository(AppDbContext db) => _db = db;

    public async Task<PriceRecord?> GetByHourAsync(DateTime utcHour)
    {
        return await _db.Prices.FirstOrDefaultAsync(x => x.TimestampHour == utcHour);
    }

    public async Task SaveAsync(PriceRecord rec)
    {
        _db.Prices.Add(rec);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<PriceRecord>> GetRangeAsync(DateTime startUtc, DateTime endUtc)
    {
        return await _db.Prices
            .Where(x => x.TimestampHour >= startUtc && x.TimestampHour <= endUtc)
            .OrderBy(x => x.TimestampHour)
            .ToListAsync();
    }
}
