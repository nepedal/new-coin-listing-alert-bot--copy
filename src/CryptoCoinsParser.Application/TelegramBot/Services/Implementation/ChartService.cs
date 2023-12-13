using CryptoCoinsParser.Coinbase.Scraper.Services;

namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

public class ChartService : IChartService
{
    private static readonly DateTime TimespanStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly BinanceApiService _binanceApiService;

    private readonly BybitApiService _bybitApiService;

    private readonly CoinbaseApiService _coinbaseApiService;

    private readonly float _dpiScale = 96 / 96;

    private readonly int _imageHeight = 1080;

    private readonly int _imageWidth = 1920;

    private readonly float _imageWidthFloat = 1920f;

    private readonly KuCoinApiService _kuCoinApiService;

    private readonly ILogger<ChartService> _logger;

    private readonly OkxApiService _okxApiService;

    public ChartService(BinanceApiService binanceApiService,
        BybitApiService bybitApiService,
        OkxApiService okxApiService,
        KuCoinApiService kuCoinApiService,
        ILogger<ChartService> logger,
        CoinbaseApiService coinbaseApiService)
    {
        _binanceApiService = binanceApiService;
        _bybitApiService = bybitApiService;
        _okxApiService = okxApiService;
        _kuCoinApiService = kuCoinApiService;
        _logger = logger;
        _coinbaseApiService = coinbaseApiService;
    }

    public InputMediaPhoto ExportChartToPng(PlotModel plot,
        DateTime salesDate,
        double totalVolume,
        double priceVolatility,
        string coinName,
        ExchangeName exchangeName)
    {
        try
        {
            var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.png");

            using var bitmap = new SKBitmap(_imageWidth, _imageHeight);

            using var canvas = new SKCanvas(bitmap);
            using var context = new SkiaRenderContext
            {
                RenderTarget = RenderTarget.PixelGraphic, SkCanvas = canvas
            };

            context.DpiScale = _dpiScale;

            canvas.Clear(plot.Background.ToSKColor());

            canvas.DecorateChart(salesDate, coinName, exchangeName, totalVolume, priceVolatility);

            ((IPlotModel)plot).Update(true);
            ((IPlotModel)plot).Render(context,
                new OxyRect(42, 238, (_imageWidthFloat - 42 - 54) / _dpiScale, 732f / _dpiScale));

            var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

            using var image = SKImage.FromBitmap(bitmap);
            if (image is null)
            {
                _logger.LogError("Error: Bitmap could not be converted to SKImage.");
                return null;
            }

            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            if (data is null)
            {
                _logger.LogError("Error: SKImage could not be encoded.");
                return null;
            }

            data.SaveTo(stream);

            stream.Position = 0;
            var file = new InputFileStream(stream, path);

            return new InputMediaPhoto(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while exporting the chart to PNG.");
        }

        return null;
    }

    public async Task<InputMediaPhoto> GenerateChartAsync(ExchangeName exchange, Coin coin)
    {
        return exchange switch
        {
            ExchangeName.Kucoin => await GetChart(_kuCoinApiService.GetCandlesticksAsync, coin.Name,
                ExchangeName.Kucoin),
            ExchangeName.Binance => await GetBinanceCoinChart(coin),
            ExchangeName.Coinbase => await GetCoinbaseChart(coin.Name),
            ExchangeName.Bybit => await GetChart(_bybitApiService.GetCandlesticksAsync, coin.Name, ExchangeName.Bybit),
            ExchangeName.Okx => await GetChart(_okxApiService.GetCandlesticksAsync, coin.Name, ExchangeName.Okx),
            ExchangeName.None => throw new ArgumentOutOfRangeException(nameof(exchange), exchange,
                "Unsupported exchange."),
            _ => throw new ArgumentOutOfRangeException(nameof(exchange), exchange, "Unsupported exchange.")
        };
    }

    private async Task<InputMediaPhoto> GetBinanceCoinChart(Coin coin)
    {
        var candlesticks = await _binanceApiService.GetCandlesticksAsync(
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero), coin.Name, 1);

        var initialCandlesticks = candlesticks.Take(30).ToList();

        var salesDate = TimespanStart.AddMilliseconds(candlesticks.First().OpenTime).ToUniversalTime();

        var plot = FinancialChartCreator.GetCandlestickPlot(initialCandlesticks);

        var totalVolume = initialCandlesticks.Sum(candlestick => candlestick.QuoteVolume);
        var priceVolatility =
            (initialCandlesticks.Max(candlestick => candlestick.High) -
             initialCandlesticks.Min(candlestick => candlestick.Low)) /
            initialCandlesticks.Min(candlestick => candlestick.Low);

        var chart = ExportChartToPng(plot, salesDate, totalVolume, priceVolatility, coin.Name, ExchangeName.Binance);

        return chart;
    }

    private async Task<InputMediaPhoto> GetCoinbaseChart(string coinName)
    {
        var now = DateTimeOffset.UtcNow;

        var candlestickDays = await _coinbaseApiService.GetCandlesticksAsync(now, coinName, 1440);

        while (candlestickDays.Count == 300)
        {
            now -= TimeSpan.FromMinutes(1440 * 300);
            candlestickDays = await _coinbaseApiService.GetCandlesticksAsync(now, coinName, 1440);
        }

        var candlestickDay = candlestickDays.MinBy(x => x.OpenTime);

        var initialCoinDate = TimespanStart.AddHours(300) + TimeSpan.FromMilliseconds(candlestickDay.OpenTime);

        var candlestickHours = await _coinbaseApiService.GetCandlesticksAsync(initialCoinDate, coinName, 60);

        var candlestickHour = candlestickHours.MinBy(x => x.OpenTime);

        var date = TimespanStart.AddMinutes(300) + TimeSpan.FromMilliseconds(candlestickHour.OpenTime);

        var candlesticks = await _coinbaseApiService.GetCandlesticksAsync(date, coinName, 1);

        var salesDate = TimespanStart.AddMilliseconds(candlesticks.First().OpenTime).ToUniversalTime();
        var initialCandlesticks = candlesticks.Take(30).ToList();

        var plot = FinancialChartCreator.GetCandlestickPlot(initialCandlesticks);

        var totalVolume = initialCandlesticks.Sum(candlestick => candlestick.QuoteVolume);
        var priceVolatility =
            (initialCandlesticks.Max(candlestick => candlestick.High) -
             initialCandlesticks.Min(candlestick => candlestick.Low)) /
            initialCandlesticks.Min(candlestick => candlestick.Low);

        var chart = ExportChartToPng(plot, salesDate, totalVolume, priceVolatility, coinName, ExchangeName.Coinbase);

        return chart;
    }

    private async Task<InputMediaPhoto> GetChart(Func<DateTimeOffset, string, int, Task<List<Candlestick>>> getCandles,
        string coinName,
        ExchangeName exchangeName)
    {
        var now = DateTimeOffset.UtcNow;

        var candlestickWeeks = await getCandles(now, coinName, 10080);

        var candlestickWeek = candlestickWeeks.MinBy(x => x.OpenTime);

        var initialCoinDate = TimespanStart.AddDays(8) + TimeSpan.FromMilliseconds(candlestickWeek.OpenTime);

        var candlestickHours = await getCandles(initialCoinDate, coinName, 120);

        var candlestickHour = candlestickHours.MinBy(x => x.OpenTime);

        var date = TimespanStart.AddMinutes(100) + TimeSpan.FromMilliseconds(candlestickHour.OpenTime);

        var candlesticks = await getCandles(date, coinName, 1);

        while (candlesticks.Count < 30)
        {
            date = date.AddMinutes(100 - candlesticks.Count);
            candlesticks = await getCandles(date, coinName, 1);
        }

        var salesDate = TimespanStart.AddMilliseconds(candlesticks.First().OpenTime).ToUniversalTime();
        var initialCandlesticks = candlesticks.Take(30).ToList();

        var plot = FinancialChartCreator.GetCandlestickPlot(initialCandlesticks);

        var totalVolume = initialCandlesticks.Sum(candlestick => candlestick.QuoteVolume);
        var priceVolatility =
            (initialCandlesticks.Max(candlestick => candlestick.High) -
             initialCandlesticks.Min(candlestick => candlestick.Low)) /
            initialCandlesticks.Min(candlestick => candlestick.Low);

        var chart = ExportChartToPng(plot, salesDate, totalVolume, priceVolatility, coinName, exchangeName);

        return chart;
    }
}
