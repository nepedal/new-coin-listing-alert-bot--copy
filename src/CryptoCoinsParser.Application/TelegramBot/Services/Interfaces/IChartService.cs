namespace CryptoCoinsParser.Application.TelegramBot.Services.Interfaces;

public interface IChartService
{
    InputMediaPhoto ExportChartToPng(PlotModel plot,
        DateTime salesDate,
        double totalVolume,
        double priceVolatility,
        string coinName,
        ExchangeName exchangeName);

    Task<InputMediaPhoto> GenerateChartAsync(ExchangeName exchange, Coin coin);
}
