namespace CryptoCoinsParser.Scrapers.Interfaces;

public interface IScraper
{
    Task<List<AnnouncementDto>> GetLatest();
}
