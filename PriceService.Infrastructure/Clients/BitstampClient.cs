using System.Text.Json;

namespace PriceService.Infrastructure.Clients;

public class BitstampClient : IBitstampClient
{
    private readonly HttpClient _http;
    public BitstampClient(HttpClient http) => _http = http;

    public async Task<double?> GetClosePriceAsync(DateTime utcHour)
    {
        var epoch = new DateTimeOffset(utcHour).ToUnixTimeSeconds();
        var url = $"https://www.bitstamp.net/api/v2/ohlc/btcusd/?step=3600&limit=1&start={epoch}";
        try
        {
            var json = await _http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("data", out var data) &&
                data.TryGetProperty("ohlc", out var arr) &&
                arr.ValueKind == JsonValueKind.Array &&
                arr.GetArrayLength() > 0)
            {
                var item = arr[0];
                if (item.TryGetProperty("close", out var closeEl) && closeEl.TryGetDouble(out var close))
                    return close;
            }
        }
        catch {}
        return null;
    }
}
