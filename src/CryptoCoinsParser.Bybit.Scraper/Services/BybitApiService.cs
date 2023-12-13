namespace CryptoCoinsParser.Bybit.Scraper.Services;

public class BybitApiService
{
    private readonly string _baseUrl = "https://api.bybit.com";

    private readonly HttpClient _client;

    private readonly Dictionary<int, string> _intervalConvertor = new()
    {
        { 1, "1" }, { 3, "3" }, { 5, "5" },
        { 15, "15" }, { 30, "30" }, { 60, "60" },
        { 120, "120" }, { 240, "240" }, { 360, "360" },
        { 720, "720" }, { 1440, "D" }, { 43830, "M" },
        { 10080, "W" }
    };

    private readonly int _limit = 1000;

    public readonly ExchangeName Exchange = ExchangeName.Bybit;

    public BybitApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Candlestick>> GetCandlesticksAsync(DateTimeOffset announcementDate,
        string coinName,
        int interval)
    {
        var announcementDateTimestamp = announcementDate.ToUnixTimeMilliseconds();

        var symbol = $"{coinName}USDT";

        var actualInterval = _intervalConvertor[interval];

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_baseUrl}/v5/market/kline?category=spot&symbol={symbol}&interval={actualInterval}&limit={_limit}&end={announcementDateTimestamp}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var candlesticks = JsonConvert.DeserializeObject<dynamic>(json)?["result"]["list"];

        return ((JArray)candlesticks).ToArray().
            Select(item => new Candlestick
            {
                OpenTime = long.Parse(item[0].ToString()),
                Open = double.Parse(item[1].ToString()),
                High = double.Parse(item[2].ToString()),
                Low = double.Parse(item[3].ToString()),
                Close = double.Parse(item[4].ToString()),
                QuoteVolume = double.Parse(item[6].ToString())
            }).
            OrderBy(x => x.OpenTime).
            ToList();
    }
}
