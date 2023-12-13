namespace CryptoCoinsParser.Application.TelegramBot.Extensions;

public static class StringExtensions
{
    public static string WithoutLastStep(this string text)
    {
        if (IsNullOrEmpty(text))
        {
            return Empty;
        }

        var commands = text.Split(Command);
        return Join(Command, commands.Take(commands.Length - 1));
    }

    public static string PrependSlash(this string text)
    {
        if (IsNullOrEmpty(text))
        {
            return Empty;
        }

        return Concat("/", text);
    }

    public static string RemoveStep(this string text, string step)
    {
        return Join(Command, text.Split(Command).Where(x => !x.StartsWith(step)));
    }

    public static string ToLocalizedTemplateId(this string name)
    {
        return Concat(name, Thread.CurrentThread.CurrentUICulture);
    }
}
