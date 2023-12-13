namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface ITelegramBotRequestLimiterService
{
    Task StopGettingRequestsAsync(long telegramUserId);

    Task AllowGettingRequestsAsync(long telegramUserId);

    Task<bool> IsAllowedRequestsAsync(long telegramUserId);
}
