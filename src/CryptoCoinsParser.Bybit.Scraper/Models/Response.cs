namespace CryptoCoinsParser.Bybit.Scraper.Models;

public class Response
{
    public int RetCode { get; set; }

    public string RetMsg { get; set; }

    public Result Result { get; set; }

    public object RetExtInfo { get; set; }

    public long Time { get; set; }
}
