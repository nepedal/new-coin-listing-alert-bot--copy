namespace CryptoCoinsParser.Application.TelegramBot.Models;

public sealed record MarkupModel
{
    public MarkupModel(string text, List<IEnumerable<InlineKeyboardButton>> buttons = null)
    {
        Buttons = buttons;
        Text = text;
    }

    public string Text { get; set; }

    public List<IEnumerable<InlineKeyboardButton>> Buttons { get; set; }
}
