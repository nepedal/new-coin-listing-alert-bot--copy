namespace CryptoCoinsParser.Binance.Scraper.Models;

public class BinanceArticleDto
{
    public string Code { get; set; }

    public object Message { get; set; }

    public object MessageDetail { get; set; }

    public Data Data { get; set; }

    public bool Success { get; set; }
}
