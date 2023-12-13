namespace CryptoCoinsParser.Binance.Scraper.Services;

public class BinanceApiService
{
    private readonly string _baseUrl = "https://api.binance.com/api";

    private readonly HttpClient _client;

    private readonly Dictionary<int, string> _intervalConvertor = new()
    {
        { 1, "1m" }, { 3, "3m" }, { 5, "5m" },
        { 15, "15m" }, { 30, "30m" }, { 60, "1h" },
        { 120, "2h" }, { 240, "4h" }, { 360, "6h" },
        { 480, "8h" }, { 720, "12h" }, { 1440, "1d" },
        { 4320, "3d" }, { 10080, "1w" }, { 43830, "1M" }
    };

    private readonly int _limit = 1000;

    public readonly ExchangeName Exchange = ExchangeName.Binance;

    public BinanceApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Candlestick>> GetCandlesticksAsync(DateTimeOffset announcementDate,
        string coinName,
        int interval)
    {
        var startTime = announcementDate.ToUnixTimeMilliseconds();

        var symbol = $"{coinName}USDT";

        var actualInterval = _intervalConvertor[interval];

        var request = new HttpRequestMessage(HttpMethod.Get,
            $"{_baseUrl}/v3/klines?symbol={symbol}&interval={actualInterval}&limit={_limit}&startTime={startTime}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var candlesticks = JsonConvert.DeserializeObject<dynamic>(json);

        return ((JArray)candlesticks).ToArray().
            Select(item => new Candlestick
            {
                OpenTime = long.Parse(item[0].ToString()),
                Open = double.Parse(item[1].ToString()),
                High = double.Parse(item[2].ToString()),
                Low = double.Parse(item[3].ToString()),
                Close = double.Parse(item[4].ToString()),
                QuoteVolume = double.Parse(item[8].ToString())
            }).
            ToList();
    }
}
