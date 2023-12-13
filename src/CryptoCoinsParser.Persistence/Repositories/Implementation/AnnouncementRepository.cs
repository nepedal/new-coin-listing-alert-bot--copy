namespace CryptoCoinsParser.Persistence.Repositories.Implementation;

public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly TelegramBotContext _context;

    public AnnouncementRepository(TelegramBotContext context)
    {
        _context = context;
    }

    public async Task CreateAnnouncementRangeAsync(IEnumerable<Announcement> announcements)
    {
        _context.Set<Announcement>().AddRange(announcements);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Announcement>> GetAnnouncementAsync()
    {
        return await _context.Set<Announcement>().ToListAsync();
    }

    public IQueryable<Announcement> Queryable()
    {
        return _context.Set<Announcement>().AsNoTracking();
    }
}
