namespace CryptoCoinsParser.Scrapers.Utils;

public static class FinancialChartCreator
{
    private static readonly DateTime TimespanStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static readonly OxyColor DarkGrey = OxyColor.FromRgb(79, 79, 79);

    private static readonly OxyColor LightGrey = OxyColor.FromRgb(189, 189, 189);

    private static readonly OxyColor Green = OxyColor.FromRgb(14, 203, 129);

    private static readonly OxyColor Red = OxyColor.FromRgb(246, 70, 93);

    private static readonly int DefaultFontSize = 18;

    public static PlotModel GetCandlestickPlot(IEnumerable<Candlestick> candlesticks)
    {
        var pm = new PlotModel
        {
            TextColor = DarkGrey,
            Background = OxyColors.White,
            PlotAreaBorderColor = LightGrey,
            DefaultFont = "Inter",
            DefaultFontSize = DefaultFontSize
        };

        var timeSpanAxis1 = new DateTimeAxis
        {
            Position = AxisPosition.Bottom,
            StringFormat = "HH:mm",
            MajorGridlineColor = LightGrey,
            MinorGridlineColor = LightGrey,
            TicklineColor = OxyColors.White,
            MajorGridlineStyle = LineStyle.Dash,
            IntervalType = DateTimeIntervalType.Minutes,
            MajorStep = 1.0 / 24 / 2 / 15, // Set the step size to 2 minutes
            FontSize = DefaultFontSize,
            FontWeight = 400
        };

        var linearAxis1 = new LinearAxis
        {
            Position = AxisPosition.Left,
            MajorGridlineColor = LightGrey,
            MinorGridlineColor = LightGrey,
            TicklineColor = OxyColors.White,
            MajorGridlineStyle = LineStyle.Dash,
            FontSize = DefaultFontSize,
            FontWeight = 400
        };

        var items = candlesticks.Select(x =>
            {
                var date = TimespanStart.AddMilliseconds(x.OpenTime).ToUniversalTime();
                return new HighLowItem(DateTimeAxis.ToDouble(date), x.High, x.Low, x.Open, x.Close);
            }).
            ToList();

        var series = new CandleStickSeries
        {
            IncreasingColor = Green,
            DecreasingColor = Red,
            ItemsSource = items
        };

        timeSpanAxis1.Maximum = items.Last().X;

        linearAxis1.Maximum = items.Max(x => x.High);
        linearAxis1.Minimum = items.Min(x => x.Low);

        pm.Axes.Add(timeSpanAxis1);
        pm.Axes.Add(linearAxis1);
        pm.Series.Add(series);

        return pm;
    }
}
