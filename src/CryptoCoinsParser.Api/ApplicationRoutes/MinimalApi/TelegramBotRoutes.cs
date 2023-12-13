using CryptoCoinsParser.Persistence.Repositories.Interfaces;

namespace CryptoCoinsParser.Api.ApplicationRoutes.MinimalApi;

public static class TelegramBotRoutes
{
    public static void MapTelegramBotRoutes(this WebApplication app)
    {
        app.MapPost("handle-command",
                async (RequestWrapper<Update> request,
                    [FromServices] ICommandDispatcher dispatcher,
                    [FromServices] IUserRepository userRepository,
                    [FromServices] ILogger<WebApplication> logger) =>
                {
                    var update = request.Value;
                    try
                    {
                        var chatId = update.GetTelegramUserIdNullable();
                        if (chatId == null)
                        {
                            chatId = update.GetDeletedTelegramUserIdNullable();
                            if (chatId != null)
                            {
                                await userRepository.DeleteUserByTelegramUserIdAsync(chatId.Value);
                            }

                            return Results.Ok();
                        }

                        await dispatcher.DispatchAsync(update);
                    }
                    catch (Exception e)
                    {
                        logger.LogCritical(e, "Failed to execute command, request = {0}",
                            JsonConvert.SerializeObject(update));
                    }

                    return Results.Ok();
                }).
            AllowAnonymous().
            ExcludeFromDescription();
    }
}
