namespace CryptoCoinsParser.Scrapers.Utils;

public static class SkiaImageCreator
{
    private static readonly string TitleFont = "Roboto";

    private static readonly string FooterFont = "Inter";

    private static readonly string IconFont = "JetBrainsMono Nerd Font";

    private static readonly string Watermark = "tg:nepedal";

    private static readonly int TitleFontSize = 32;

    private static readonly int SubtitleFontSize = 28;

    private static readonly int FooterFontSize = 24;

    private static readonly int ImageWidth = 1920;

    private static readonly int ImageHeight = 1080;

    private static readonly SKColor SubtitleTextColor = SKColor.FromHsl(0f, 0f, 0f, 153);

    internal static void DecorateChart(this SKCanvas canvas,
        DateTime announcementDate,
        string coinName,
        ExchangeName exchange,
        double tradingVolume,
        double priceVolatility)
    {
        canvas.DrawWatermark(Watermark);
        canvas.DrawTitle(coinName, exchange);
        canvas.DrawSubtitle(announcementDate);
        canvas.DrawFooter(tradingVolume, priceVolatility);
    }

    private static void DrawWatermark(this SKCanvas canvas, string watermark)
    {
        using var paint = new SKPaint();
        paint.IsStroke = false;
        paint.FakeBoldText = false;
        paint.SubpixelText = true;
        paint.LcdRenderText = true;
        paint.Style = SKPaintStyle.Fill;
        paint.IsAntialias = true;
        paint.Color = SKColor.FromHsl(225, 2, 55, 64);
        paint.Typeface = SKTypeface.FromFamilyName(
            TitleFont,
            SKFontStyleWeight.Bold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        paint.TextSize = 210;
        paint.TextAlign = SKTextAlign.Center;
        paint.TextEncoding = SKTextEncoding.Utf8;

        var x = ImageWidth / 2f;
        var y = ImageHeight / 2f;

        canvas.Save();

        canvas.Translate(x, y);

        canvas.RotateDegrees(-20);

        canvas.DrawText(watermark, 0, 238f / 2, paint);

        canvas.Restore();
    }

    private static void DrawTitle(this SKCanvas canvas, string coinName, ExchangeName exchange)
    {
        using var titlePaint = new SKPaint();
        titlePaint.IsAntialias = true;
        titlePaint.Color = SKColors.Black;
        titlePaint.Typeface = SKTypeface.FromFamilyName(
            TitleFont,
            SKFontStyleWeight.Bold,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        titlePaint.TextSize = TitleFontSize;
        titlePaint.TextAlign = SKTextAlign.Left;
        titlePaint.TextEncoding = SKTextEncoding.Utf8;

        canvas.DrawText($"{exchange.ToString()} {coinName}-USDT", 106f, 100f, titlePaint);

        using var linePaint = new SKPaint();
        linePaint.Color = SKColor.FromHsl(0, 0, 94);
        linePaint.StrokeWidth = 4f;

        var start = new SKPoint(0f, 200f);
        var end = new SKPoint(ImageWidth, 200f);

        canvas.DrawLine(start, end, linePaint);
    }

    private static void DrawSubtitle(this SKCanvas canvas, DateTime announcementDate)
    {
        using var subtitlePaint = new SKPaint();
        subtitlePaint.IsAntialias = true;
        subtitlePaint.Color = SubtitleTextColor;
        subtitlePaint.Typeface = SKTypeface.FromFamilyName(
            TitleFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        subtitlePaint.TextSize = SubtitleFontSize;
        subtitlePaint.TextAlign = SKTextAlign.Left;
        subtitlePaint.TextEncoding = SKTextEncoding.Utf8;

        canvas.DrawText("First 30 Minutes Spot Trading", 106f, 165f, subtitlePaint);

        using var dateSubtitlePaint = new SKPaint();
        dateSubtitlePaint.IsAntialias = true;
        dateSubtitlePaint.Color = SubtitleTextColor;
        dateSubtitlePaint.Typeface = SKTypeface.FromFamilyName(
            TitleFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        dateSubtitlePaint.TextSize = SubtitleFontSize;
        dateSubtitlePaint.TextAlign = SKTextAlign.Right;
        dateSubtitlePaint.TextEncoding = SKTextEncoding.Utf8;

        var endDate = announcementDate.AddMinutes(30);

        var dateSubtitle = $"{announcementDate:d MMM - yyyy, h.mm tt} - {endDate:h.mm tt}";

        canvas.DrawText(dateSubtitle, ImageWidth - 82f, 165f, dateSubtitlePaint);

        using var calendarPaint = new SKPaint();
        calendarPaint.IsAntialias = true;
        calendarPaint.Color = SKColor.FromHsl(0f, 0f, 0f);
        calendarPaint.Typeface = SKTypeface.FromFamilyName(
            IconFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        calendarPaint.TextSize = 28;
        calendarPaint.TextAlign = SKTextAlign.Right;
        calendarPaint.TextEncoding = SKTextEncoding.Utf16;

        canvas.DrawText("\udb80\udced", ImageWidth - dateSubtitlePaint.MeasureText(dateSubtitle) - 102f, 165f,
            calendarPaint);
    }

    private static void DrawFooter(this SKCanvas canvas, double tradingVolume, double priceVolatility)
    {
        using var circlePaint = new SKPaint();
        circlePaint.IsAntialias = true;
        circlePaint.Color = SKColor.FromHsl(0f, 0f, 85f);
        circlePaint.Typeface = SKTypeface.FromFamilyName(
            IconFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        circlePaint.TextSize = 33;
        circlePaint.TextAlign = SKTextAlign.Center;
        circlePaint.TextAlign = SKTextAlign.Left;
        circlePaint.TextEncoding = SKTextEncoding.Utf16;

        using var tradingVolumePaint = new SKPaint();
        tradingVolumePaint.IsAntialias = true;
        tradingVolumePaint.Color = SKColors.Black;
        tradingVolumePaint.Typeface = SKTypeface.FromFamilyName(
            FooterFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        tradingVolumePaint.TextSize = FooterFontSize;

        tradingVolumePaint.TextAlign = SKTextAlign.Left;
        tradingVolumePaint.TextEncoding = SKTextEncoding.Utf8;

        var tradingVolumeText = $"Trading Volume: {FormatNumber(tradingVolume)} USDT";

        canvas.DrawText(tradingVolumeText, 135f, ImageHeight - 55f, tradingVolumePaint);
        canvas.DrawText("\uea71", 105f, ImageHeight - 52f, circlePaint);

        using var priceVolatilityPaint = new SKPaint();
        priceVolatilityPaint.IsAntialias = true;
        priceVolatilityPaint.Color = SKColor.FromHsl(0f, 0f, 0f);
        priceVolatilityPaint.Typeface = SKTypeface.FromFamilyName(
            FooterFont,
            SKFontStyleWeight.Normal,
            SKFontStyleWidth.Normal,
            SKFontStyleSlant.Upright
        );

        priceVolatilityPaint.TextSize = FooterFontSize;
        priceVolatilityPaint.TextAlign = SKTextAlign.Left;
        priceVolatilityPaint.TextEncoding = SKTextEncoding.Utf8;

        var tradingVolumeWidth = tradingVolumePaint.MeasureText(tradingVolumeText);

        canvas.DrawText($"Price Volatility: {priceVolatility:P1}", tradingVolumeWidth + 190f, ImageHeight - 55f,
            priceVolatilityPaint);

        canvas.DrawText("\uea71", tradingVolumeWidth + 160f, ImageHeight - 52f, circlePaint);
    }

    private static string FormatNumber(double num)
    {
        return num switch
        {
            >= 1000000000000 => (num / 1000000000000D).ToString("0.#") + "T",
            >= 1000000000 => (num / 1000000000D).ToString("0.#") + "B",
            >= 1000000 => (num / 1000000D).ToString("0.#") + "M",
            >= 1000 => (num / 1000D).ToString("0.#") + "K",
            _ => num.ToString("0.#")
        };
    }
}
