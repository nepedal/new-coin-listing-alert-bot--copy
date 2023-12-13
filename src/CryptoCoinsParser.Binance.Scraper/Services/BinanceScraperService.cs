using System.Text.RegularExpressions;

namespace CryptoCoinsParser.Binance.Scraper.Services;

public class BinanceScraperService : IScraper
{
    private readonly Regex _bracketsRegex = new(@"(?<=\().*?(?=\))");

    private readonly HttpClient _client;

    private readonly Regex _coinsRegex = new(@"\b[A-Z]{3,}\b");

    public BinanceScraperService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<AnnouncementDto>> GetLatest()
    {
        var request = new HttpRequestMessage(HttpMethod.Get,
            "https://www.binance.com/bapi/composite/v1/public/cms/article/catalog/list/query?catalogId=48&pageNo=1&pageSize=10");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        var articles = JsonConvert.DeserializeObject<BinanceArticleDto>(json);

        var articleTitles = articles.Data.Articles.Where(article => article.Title.Contains("Binance Will List ") ||
                                                                    article.Title.Contains("Binance Will Open Trading ")).
            Select(article =>
                new AnnouncementDto
                {
                    Message = article.Title,
                    Coins = new List<CoinDto>()
                }).
            ToList();

        var coins = articleTitles.SelectMany(tradingDataItem =>
            {
                var matches = _bracketsRegex.Matches(tradingDataItem.Message);
                if (matches.Any())
                {
                    return matches.Select(match => new CoinDto { Name = match.Groups[0].Value });
                }

                matches = _coinsRegex.Matches(tradingDataItem.Message);
                if (matches.Any() && matches.Count == 1)
                {
                    return matches.Select(match => new CoinDto { Name = match.Groups[0].Value });
                }

                return new HashSet<CoinDto>();
            }).
            ToHashSet();

        foreach (var coin in coins)
        {
            articleTitles.FirstOrDefault(x => x.Message.Contains(coin.Name)).Coins.Add(coin);
            articleTitles.FirstOrDefault(x => x.Message.Contains(coin.Name)).Exchange = ExchangeName.Binance;
        }

        return articleTitles;
    }
}
