namespace CryptoCoinsParser.Scrapers.Models;

public record Candlestick
{
    public long OpenTime { get; set; }

    public double Open { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public double Close { get; set; }

    public double QuoteVolume { get; set; }
}
