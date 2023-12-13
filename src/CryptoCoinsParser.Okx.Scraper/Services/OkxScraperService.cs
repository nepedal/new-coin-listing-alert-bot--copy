namespace CryptoCoinsParser.Okx.Scraper.Services;

public class OkxScraperService : IScraper
{
    private readonly HttpClient _client;

    private readonly Regex _coinsRegex = new(@"\b(?!USDT|OKX|UTC\b)[A-Z]+\b");

    private readonly ILogger<OkxScraperService> _logger;

    private readonly Uri _okxUrl = new("https://www.okx.com/help/section/announcements-latest-announcements");

    public OkxScraperService(HttpClient client, ILogger<OkxScraperService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<List<AnnouncementDto>> GetLatest()
    {
        try
        {
            var html = await _client.GetStringAsync(_okxUrl);
            var xpath = "//li[@class='index_article__15dX1']";
            var keyWords = new[] { " for spot ", " will launch " };

            var announcements = GetTextFromXPath(html, xpath, keyWords);

            var coins = announcements.Select(tradingDataItem => _coinsRegex.Match(tradingDataItem.Message)).
                Where(match => match.Success).
                Select(match => new CoinDto { Name = match.Groups[0].Value }).
                ToHashSet();

            foreach (var coin in coins)
            {
                announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Coins.Add(coin);
                announcements.FirstOrDefault(x => x.Message.Contains(coin.Name)).Exchange = ExchangeName.Okx;
            }

            return announcements;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get latest announcements from OKX.");
            return new List<AnnouncementDto>();
        }
    }

    private List<AnnouncementDto> GetTextFromXPath(string html, string xpath, IEnumerable<string> keyWords)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var nodes = document.DocumentNode.SelectNodes(xpath);

        if (nodes is null)
        {
            _logger.LogInformation("No nodes matched the provided XPath.");
            return new List<AnnouncementDto>();
        }

        var divs = nodes.SelectMany(node => node.Descendants("div")).ToList();

        if (!divs.Any())
        {
            _logger.LogInformation("No divs matched the provided XPath.");
            return new List<AnnouncementDto>();
        }

        var texts = divs.Select(div => new AnnouncementDto { Message = div.InnerText, Coins = new List<CoinDto>() }).
            Where(text => keyWords.Any(text.Message.Contains) && !text.Message.Contains("UTC")).
            ToList();

        return texts;
    }
}
