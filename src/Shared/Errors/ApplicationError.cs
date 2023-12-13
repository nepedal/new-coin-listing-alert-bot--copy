using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Shared.Errors;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ApplicationError
{
    public ApplicationError(ApplicationErrorCode code, string message)
    {
        Message = message;
        Code = code;
    }

    [JsonRequired]
    public ApplicationErrorCode Code { get; }

    [JsonRequired]
    public string Message { get; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
    }
}

public class ApplicationError<TAdditionalData> : ApplicationError
{
    public readonly TAdditionalData AdditionalData;

    public ApplicationError(ApplicationErrorCode code, string message, TAdditionalData additionalData) : base(code,
        message)
    {
        AdditionalData = additionalData;
    }
}
