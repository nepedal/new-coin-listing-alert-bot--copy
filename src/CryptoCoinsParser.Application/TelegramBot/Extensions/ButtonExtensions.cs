namespace CryptoCoinsParser.Application.TelegramBot.Extensions;

public static class ButtonExtensions
{
    public static List<IEnumerable<InlineKeyboardButton>> WithRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        InlineKeyboardButton btn)
    {
        markupButtons.Add(new[] { btn });

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        IEnumerable<InlineKeyboardButton> btns)
    {
        markupButtons.Add(btns);

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        params InlineKeyboardButton[] btns)
    {
        markupButtons.Add(btns);

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithCallbackButtonRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        string title,
        string callbackData)
    {
        markupButtons.Add(new[] { WithCallbackData(title, callbackData) });

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithUrlButtonRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        string title,
        string url)
    {
        markupButtons.Add(new[] { WithUrl(title, url) });

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithWebApp(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        string title,
        string url)
    {
        markupButtons.Add(new[]
        {
            InlineKeyboardButton.WithWebApp(title, new WebAppInfo
            {
                Url = url
            })
        });

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> InsertCallbackButtonsRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        int index,
        params CallbackButton[] callbackButtons)
    {
        var buttonsToInsert =
            callbackButtons.Select(x => WithCallbackData(x.Title, x.CallbackData));

        markupButtons.Insert(index, buttonsToInsert);

        return markupButtons;
    }

    public static List<IEnumerable<InlineKeyboardButton>> WithCallbackButtonsRow(
        this List<IEnumerable<InlineKeyboardButton>> markupButtons,
        params CallbackButton[] callbackButtons)
    {
        var buttonsToAdd =
            callbackButtons.Select(x => WithCallbackData(x.Title, x.CallbackData));

        markupButtons.Add(buttonsToAdd);

        return markupButtons;
    }

    public static InlineKeyboardMarkup ToMarkup(this IEnumerable<IEnumerable<InlineKeyboardButton>> markup)
    {
        if (markup == null)
        {
            return InlineKeyboardMarkup.Empty();
        }

        return new InlineKeyboardMarkup(markup);
    }
}

public record struct CallbackButton(string Title, string CallbackData)
{
    public string Title { get; init; } = Title;

    public string CallbackData { get; init; } = CallbackData;
}
