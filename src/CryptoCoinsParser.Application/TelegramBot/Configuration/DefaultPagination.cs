namespace CryptoCoinsParser.Application.TelegramBot.Configuration;

public sealed record DefaultPagination
{
    public const string Section = "DefaultPagination";

    public PaginationParams BackgroundWorkerUsers { get; set; }
}
