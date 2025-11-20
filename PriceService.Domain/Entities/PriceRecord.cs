namespace PriceService.Domain.Entities;

public class PriceRecord
{
    public int Id { get; set; }
    public DateTime TimestampHour { get; set; }
    public double AggregatedPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
