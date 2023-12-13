using System.Text.RegularExpressions;

namespace CryptoCoinsParser.KuCoin.Scraper.Services;

public class KuCoinScraperService : IScraper
{
    private readonly HttpClient _client;

    private readonly Regex _coinsRegex = new(@"(?<=\().*?(?=\))");

    public KuCoinScraperService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<AnnouncementDto>> GetLatest()
    {
        var response =
            await _client.GetAsync(
                "https://www.kucoin.com/_api/cms/articles?page=1&pageSize=20&category=new-listings&lang=en_US");

        var jsonContent = await response.Content.ReadAsStringAsync();

        var announcementResponse =
            JsonConvert.DeserializeObject<ListAnnouncementResponse>(jsonContent);

        var announcements = announcementResponse.Items.Where(ar => ar.Title.Contains(" Gets Listed ") && !ar.Title.Contains("NFT")).
            Select(x =>
                new AnnouncementDto
                {
                    Message = x.Title,
                    Coins = new List<CoinDto>()
                }).
            ToList();

        var coins = announcements.Select(tradingDataItem => _coinsRegex.Match(tradingDataItem.Message)).
            Where(match => match.Success).
            Select(match => new CoinDto { Name = match.Groups[0].Value }).
            ToHashSet();

        foreach (var coin in coins)
        {
            announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Coins.Add(coin);
            announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Exchange = ExchangeName.Kucoin;
        }

        return announcements;
    }
}
