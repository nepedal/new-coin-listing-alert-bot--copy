namespace CryptoCoinsParser.Binance.Scraper.Models;

public class Article
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Title { get; set; }

    public object Body { get; set; }

    public object Type { get; set; }

    public object CatalogId { get; set; }

    public object CatalogName { get; set; }

    public object PublishDate { get; set; }
}
