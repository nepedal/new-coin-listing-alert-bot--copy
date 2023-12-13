namespace CryptoCoinsParser.Application.TelegramBot.Extensions;

public static class CultureExtensions
{
    public static void SetUiLanguage(this Thread currentThread, string uiLanguageId)
    {
        currentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(uiLanguageId);
    }
}
