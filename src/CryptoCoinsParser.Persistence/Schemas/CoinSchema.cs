namespace CryptoCoinsParser.Persistence.Schemas;

public class CoinSchema
{
    public const string Table = "coins";

    [UsedImplicitly]
    public class Columns : ColumnsBase
    {
        public const string Name = "name";
    }
}
