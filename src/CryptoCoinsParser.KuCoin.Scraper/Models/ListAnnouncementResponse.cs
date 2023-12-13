namespace CryptoCoinsParser.KuCoin.Scraper.Models;

public class ListAnnouncementResponse
{
    public bool Success { get; set; }

    public int Code { get; set; }

    public string Msg { get; set; }

    public long Timestamp { get; set; }

    public int TotalNum { get; set; }

    public List<Announcement> Items { get; set; }
}
