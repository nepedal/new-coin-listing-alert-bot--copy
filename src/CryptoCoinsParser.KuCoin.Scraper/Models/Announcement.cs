namespace CryptoCoinsParser.KuCoin.Scraper.Models;

public class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Summary { get; set; }

    public string Path { get; set; }

    public List<string> Tags { get; set; }

    public List<string> Images { get; set; }

    public int Hot { get; set; }

    public int Stick { get; set; }

    public DateTime PublishAt { get; set; }

    public DateTime FirstPublishAt { get; set; }

    public bool IsNew { get; set; }

    public List<Category> Categories { get; set; }

    public bool IsNewData { get; set; }

    public long PublishTs { get; set; }

    public bool IgnoreTemplate { get; set; }
}
