namespace CryptoCoinsParser.Application.TelegramBot.Extensions;

public static class DateTimeExtensions
{
    public const string UtcIdentificator = "UTC";

    public static DateTime ToUserTimeZone(this DateTime dateTime, string timeZoneId)
    {
        if (timeZoneId == null)
        {
            return dateTime;
        }

        return TimeZoneInfo.ConvertTime(dateTime, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
    }

    public static string ToLongLocalizedDateString(this DateTime dateTime)
    {
        return dateTime.ToString("D", CultureInfo.CurrentUICulture);
    }

    public static string ToShortLocalizedTimeString(this DateTime dateTime)
    {
        return dateTime.ToString("t", CultureInfo.CurrentUICulture);
    }
}
