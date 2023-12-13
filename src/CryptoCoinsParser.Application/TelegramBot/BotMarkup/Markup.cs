namespace CryptoCoinsParser.Application.TelegramBot.BotMarkup;

public static class Markup
{
    public static InlineKeyboardButton CancelButton =>
        WithCallbackData(ButtonTextCancel, CancelOperationCallback.Command);

    public static List<IEnumerable<InlineKeyboardButton>> InlineButtons => new();

    public static InlineKeyboardButton GoToMenuButton => WithCallbackData(ButtonTextGoToMenu, GoToMenuCallback.Command);

    public static List<IEnumerable<InlineKeyboardButton>> GetKeyboardButtons(MarkupDetails markupDetails)
    {
        var callbackData = markupDetails.AdditionalCallbackData;

        //if additionalCallbackData != empty, add row for button BACK
        if (!IsNullOrEmpty(callbackData))
        {
            callbackData += Command;
        }

        IEnumerable<IEnumerable<ButtonDetail>> chunkedList;

        if (markupDetails.Pagination != null)
        {
            chunkedList = markupDetails.ButtonDetails.Take(markupDetails.Pagination.Limit).Chunk(markupDetails.ButtonsInRow);
        }
        else
        {
            chunkedList = markupDetails.ButtonDetails.Chunk(markupDetails.ButtonsInRow);
        }

        var inlineButtons = chunkedList.Select(batch => batch.Select(detail => new InlineKeyboardButton(detail.Text)
            {
                CallbackData = Concat(callbackData, markupDetails.CallbackCommand, SubCommand, detail.Id)
            })).
            ToList();

        var dataWithOutLatestStep = callbackData.WithoutLastStep();

        if (IsNullOrEmpty(callbackData))
        {
            return inlineButtons;
        }

        if (markupDetails.AddBackButton)
        {
            inlineButtons.Add(new[]
            {
                BackButton(dataWithOutLatestStep.WithoutLastStep()),
                CancelButton
            });
        }

        return inlineButtons;
    }

    public static InlineKeyboardButton[] BackButtonRow(string callbackData)
    {
        return new InlineKeyboardButton[]
        {
            new(ButtonTextGoBack) { CallbackData = callbackData }
        };
    }

    public static InlineKeyboardButton BackButton(string callbackData)
    {
        return WithCallbackData(ButtonTextGoBack, callbackData);
    }
}
