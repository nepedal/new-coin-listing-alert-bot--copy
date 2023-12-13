using System.Net.Http.Headers;
using CryptoCoinsParser.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryptoCoinsParser.Coinbase.Scraper.Services;

public class CoinbaseApiService
{
    private readonly string _baseUrl = "https://api.exchange.coinbase.com";

    private readonly HttpClient _client;

    // minutes to seconds
    private readonly Dictionary<int, int> _intervalConvertor = new()
    {
        { 1, 60 }, { 5, 300 }, { 15, 900 },
        { 60, 3600 }, { 360, 21600 }, { 1440, 86400 }
    };

    public readonly ExchangeName Exchange = ExchangeName.Coinbase;

    public CoinbaseApiService(HttpClient client)
    {
        _client = client;
        _client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/117.0");
    }

    // note: interval is in seconds
    public async Task<List<Candlestick>> GetCandlesticksAsync(DateTimeOffset announcementDate,
        string coinName,
        int interval)
    {
        var announcementDateTimestamp = announcementDate.ToUnixTimeSeconds();

        var symbol = $"{coinName}-USD";

        var actualInterval = _intervalConvertor[interval];

        var start = announcementDateTimestamp - actualInterval * 300;

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_baseUrl}/products/{symbol}/candles?granularity={actualInterval}&start={start}&end={announcementDateTimestamp}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var candlesticks = JsonConvert.DeserializeObject<dynamic>(json);

        return ((JArray)candlesticks!).ToArray().
            Select(item => new Candlestick
            {
                OpenTime = long.Parse(item[0].ToString()) * 1000, // to milliseconds
                Low = double.Parse(item[1].ToString()),
                High = double.Parse(item[2].ToString()),
                Open = double.Parse(item[3].ToString()),
                Close = double.Parse(item[4].ToString()),
                QuoteVolume = double.Parse(item[5].ToString())
            }).
            OrderBy(x => x.OpenTime).
            ToList();
    }
}
