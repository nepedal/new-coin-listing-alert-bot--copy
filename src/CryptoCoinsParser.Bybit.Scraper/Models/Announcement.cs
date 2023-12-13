namespace CryptoCoinsParser.Bybit.Scraper.Models;

public class Announcement
{
    public string Title { get; set; }

    public string Description { get; set; }

    public Type Type { get; set; }

    public List<string> Tags { get; set; }

    public string Url { get; set; }

    public long DateTimestamp { get; set; }

    public long StartDateTimestamp { get; set; }

    public long EndDateTimestamp { get; set; }
}
