namespace CryptoCoinsParser.KuCoin.Scraper.Services;

public class KuCoinApiService
{
    private readonly string _baseUrl = "https://api.kucoin.com/api";

    private readonly HttpClient _client;

    private readonly Dictionary<int, string> _intervalConvertor = new()
    {
        { 1, "1min" }, { 3, "3min" }, { 5, "5min" },
        { 15, "15min" }, { 30, "30min" }, { 60, "1hour" },
        { 120, "2hour" }, { 240, "4hour" }, { 360, "6hour" },
        { 480, "8hour" }, { 720, "12hour" }, { 1440, "1day" },
        { 10080, "1week" }
    };

    public readonly ExchangeName Exchange = ExchangeName.Kucoin;

    public KuCoinApiService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Candlestick>> GetCandlesticksAsync(DateTimeOffset announcementDate,
        string coinName,
        int interval)
    {
        var endTimestamp = announcementDate.ToUnixTimeSeconds();

        var startTimestamp = endTimestamp - 852037002; // 27 years

        var symbol = $"{coinName}-USDT";

        var actualInterval = _intervalConvertor[interval];

        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_baseUrl}/v1/market/candles?type={actualInterval}&symbol={symbol}&startAt={startTimestamp}&endAt={endTimestamp}");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var candlesticks = JsonConvert.DeserializeObject<dynamic>(json)?["data"];

        return ((JArray)candlesticks).ToArray().
            Select(item => new Candlestick
            {
                OpenTime = long.Parse(item[0].ToString()) * 1000, // to milliseconds 
                Open = double.Parse(item[1].ToString()),
                Close = double.Parse(item[2].ToString()),
                High = double.Parse(item[3].ToString()),
                Low = double.Parse(item[4].ToString()),
                QuoteVolume = double.Parse(item[6].ToString())
            }).
            OrderBy(x => x.OpenTime).
            ToList();
    }
}
