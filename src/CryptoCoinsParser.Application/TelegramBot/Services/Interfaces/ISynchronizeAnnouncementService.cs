namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ISynchronizeAnnouncementService
{
    Task<List<Announcement>> GetAnnouncementsAsync();
}
