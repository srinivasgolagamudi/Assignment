using System.Text.Json;

namespace PriceService.Infrastructure.Clients;

public class BitfinexClient : IBitfinexClient
{
    private readonly HttpClient _http;
    public BitfinexClient(HttpClient http) => _http = http;

    public async Task<double?> GetClosePriceAsync(DateTime utcHour)
    {
        var start = new DateTimeOffset(utcHour).ToUnixTimeMilliseconds();
        var end = start + 3600 * 1000;
        var url = $"https://api-pub.bitfinex.com/v2/candles/trade:1h:tBTCUSD/hist?start={start}&end={end}&limit=1";

        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);

            var arr = doc.RootElement;
            if (arr.ValueKind == JsonValueKind.Array && arr.GetArrayLength() > 0)
            {
                var item = arr[0];
                if (item.ValueKind == JsonValueKind.Array && item.GetArrayLength() > 2)
                {
                    if (item[2].TryGetDouble(out var close))
                        return close;
                }
            }
        }
        catch {}

        return null;
    }
}
