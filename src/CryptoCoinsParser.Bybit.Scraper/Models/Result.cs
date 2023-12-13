namespace CryptoCoinsParser.Bybit.Scraper.Models;

public class Result
{
    public int Total { get; set; }

    public List<Announcement> List { get; set; }
}
