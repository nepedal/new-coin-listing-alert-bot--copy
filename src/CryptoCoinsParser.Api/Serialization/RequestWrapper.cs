namespace CryptoCoinsParser.Api.Serialization;

public class RequestWrapper<TModel>
{
    public RequestWrapper(TModel value)
    {
        Value = value;
    }

    public TModel Value { get; }

    public static async ValueTask<RequestWrapper<TModel>> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!context.Request.HasJsonContentType())
        {
            throw new BadHttpRequestException(
                "Request content type was not a recognized JSON content type.",
                StatusCodes.Status415UnsupportedMediaType);
        }

        using var sr = new StreamReader(context.Request.Body);
        var str = await sr.ReadToEndAsync();

        return new RequestWrapper<TModel>(JsonConvert.DeserializeObject<TModel>(str));
    }
}
