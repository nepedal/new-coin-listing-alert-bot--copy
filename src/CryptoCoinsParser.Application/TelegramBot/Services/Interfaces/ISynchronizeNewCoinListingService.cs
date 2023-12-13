namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ISynchronizeNewCoinListingService
{
    Task<List<Announcement>> GetNewCoinListingAsync();

    Task<List<Announcement>> SynchronizeNewCoinListing(IEnumerable<AnnouncementDto> announcementDtos);
}
