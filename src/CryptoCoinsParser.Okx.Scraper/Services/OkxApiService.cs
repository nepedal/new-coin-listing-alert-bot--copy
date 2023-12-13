namespace CryptoCoinsParser.Okx.Scraper.Services;

public class OkxApiService
{
    private readonly string _baseUrl = "https://www.okx.com/api";

    private readonly HttpClient _client;

    private readonly Dictionary<int, string> _intervalConvertor = new()
    {
        { 1, "1m" }, { 3, "3m" }, { 5, "5m" },
        { 15, "15m" }, { 30, "30m" }, { 60, "1H" },
        { 120, "2H" }, { 240, "4H" }, { 360, "6H" },
        { 720, "12H" }, { 1440, "1D" }, { 2880, "2D" },
        { 4320, "3D" }, { 10080, "1W" }, { 43830, "1M" },
        { 131490, "3M" }
    };

    private readonly int _limit = 300;

    public readonly ExchangeName Exchange = ExchangeName.Okx;

    public OkxApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Candlestick>> GetCandlesticksAsync(DateTimeOffset announcementDate,
        string coinName,
        int interval)
    {
        var endTimestamp = announcementDate.ToUnixTimeMilliseconds();

        var symbol = $"{coinName}-USDT";

        var actualInterval = _intervalConvertor[interval];

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_baseUrl}/v5/market/history-candles?bar={actualInterval}&instId={symbol}&after={endTimestamp}&limit={_limit}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var candlesticks = JsonConvert.DeserializeObject<dynamic>(json)?["data"];

        return ((JArray)candlesticks).ToArray().
            Select(item => new Candlestick
            {
                OpenTime = long.Parse(item[0].ToString()),
                Open = double.Parse(item[1].ToString()),
                High = double.Parse(item[2].ToString()),
                Low = double.Parse(item[3].ToString()),
                Close = double.Parse(item[4].ToString()),
                QuoteVolume = double.Parse(item[7].ToString())
            }).
            OrderBy(x => x.OpenTime).
            ToList();
    }
}
