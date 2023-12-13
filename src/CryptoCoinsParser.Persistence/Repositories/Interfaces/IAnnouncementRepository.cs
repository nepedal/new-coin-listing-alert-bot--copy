namespace CryptoCoinsParser.Persistence.Repositories.Interfaces;

public interface IAnnouncementRepository
{
    Task CreateAnnouncementRangeAsync(IEnumerable<Announcement> announcements);

    Task<List<Announcement>> GetAnnouncementAsync();

    IQueryable<Announcement> Queryable();
}
