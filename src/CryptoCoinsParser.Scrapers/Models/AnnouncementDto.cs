namespace CryptoCoinsParser.Scrapers.Models;

public record AnnouncementDto
{
    public string Message { get; set; }

    public List<CoinDto> Coins { get; set; }

    public ExchangeName Exchange { get; set; }
}
