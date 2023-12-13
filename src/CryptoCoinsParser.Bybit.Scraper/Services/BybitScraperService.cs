using System.Text.RegularExpressions;

namespace CryptoCoinsParser.Bybit.Scraper.Services;

public class BybitScraperService : IScraper
{
    private readonly string _bybitUrl =
        "https://api.bybit.com/v5/announcements/index?locale=en-US&limit=15&page=1&type=new_crypto";

    private readonly HttpClient _client;

    private readonly Regex _coinsRegex = new(@"\b(?!USDT\b)[A-Z]+\b");

    public BybitScraperService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<AnnouncementDto>> GetLatest()
    {
        var response = await _client.GetAsync(_bybitUrl);

        var json = await response.Content.ReadAsStringAsync();

        var announcementResponse = JsonConvert.DeserializeObject<Response>(json);

        var announcements =
            announcementResponse.Result.List.Select(x => new AnnouncementDto { Message = x.Title, Coins = new List<CoinDto>() }).
                Where(x => x.Message.Contains("New Listing: ")).
                ToList();

        var coins = announcements.Select(tradingDataItem => _coinsRegex.Match(tradingDataItem.Message)).
            Where(match => match.Success).
            Select(match => new CoinDto { Name = match.Groups[0].Value }).
            ToHashSet();

        foreach (var coin in coins)
        {
            announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Coins.Add(coin);
            announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Exchange = ExchangeName.Bybit;
        }

        return announcements;
    }
}
