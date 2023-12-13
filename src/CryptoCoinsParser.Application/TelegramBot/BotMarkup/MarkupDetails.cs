namespace CryptoCoinsParser.Application.TelegramBot.BotMarkup;

public sealed class MarkupDetails
{
    public IReadOnlyList<ButtonDetail> ButtonDetails { get; set; }

    public string CallbackCommand { get; set; }

    public byte ButtonsInRow { get; set; } = 3;

    public bool AddFooterButtons { get; set; } = true;

    public string Next { get; set; } = ButtonText.RightArrow;

    public string Previous { get; set; } = ButtonText.LeftArrow;

    public bool AddBackButton { get; set; } = true;

    public string AdditionalCallbackData { get; set; } = Empty;

    public PaginationParams Pagination { get; set; }
}

public class ButtonDetail
{
    public ButtonDetail(string id, string text)
    {
        Id = id;
        Text = text;
    }

    public string Id { get; set; }

    public string Text { get; set; }
}
