namespace CryptoCoinsParser.Application.TelegramBot.Services.Implementation;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly ITelegramBotClient _botClient;

    private readonly BotConfiguration _botConfiguration;

    private readonly ILogger<CommandDispatcher> _logger;

    private readonly ServiceResolver<IBotCommandHandler> _serviceAccessor;

    private readonly IUserRepository _userRepository;

    public CommandDispatcher(ITelegramBotClient botClient,
        ILogger<CommandDispatcher> logger,
        ServiceResolver<IBotCommandHandler> serviceAccessor,
        BotConfiguration botConfiguration,
        IUserRepository userRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _serviceAccessor = serviceAccessor;
        _botConfiguration = botConfiguration;
        _userRepository = userRepository;
    }

    public async Task DispatchAsync(Update update)
    {
        if (update.CallbackQuery?.Data == DoNothingCallback)
        {
            return;
        }

        var commandName = update.GetCommand();

        var chat = update.GetUpdateChat();

        var telegramUserId = chat.Id;

        var commandHandler = _serviceAccessor(commandName);

        var user = await _userRepository.GetUserByTelegramUserIdAsync(telegramUserId);

        if (user is null && commandHandler?.GetType() != typeof(StartBotCommandHandler))
        {
            var message = Format(StartBotCommand, Start.Command.PrependSlash());

            await _botClient.SendTextMessageAsync(chat, message);
            return;
        }

        if (commandHandler?.GetType() == typeof(DeleteMeCommandHandler))
        {
            await ExecuteCommandAsync(commandName, update);
            return;
        }

        await ExecuteCommandAsync(commandName, update);
    }

    public async Task ExecuteCommandHandler(IBotCommandHandler commandHandler, Update update)
    {
        var chat = update.GetUpdateChat();

        var commandHandlerName = commandHandler.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command handler:{CommandName}", commandHandlerName);

            await commandHandler.Handle(update);

            _logger.LogInformation("Finished executing command:{CommandName}", commandHandlerName);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e, "Exception occured in command:{CommandName}, request body: {Body}",
                commandHandlerName, JsonConvert.SerializeObject(update));

            await _botClient.SendTextMessageAsync(chat, Format(SomethingWentWrong, _botConfiguration.ContactNickname));
        }
    }

    private async Task ExecuteCommandAsync(string name, Update update)
    {
        var commandHandler = _serviceAccessor(name);

        if (commandHandler is null)
        {
            // if (cachedUser.PracticeLanguageId is null)
            // {
            //     commandHandler = _serviceAccessor(SetMeetingLanguage.Command.PrependSlash());
            // }
            // else if (cachedUser.PracticeLanguageLevel is null)
            // {
            //     commandHandler = _serviceAccessor(SetMeetingLanguageLevel.Command.PrependSlash());
            // }
            // else if (cachedUser.Gender is null)
            // {
            //     commandHandler = _serviceAccessor(SetGender.Command.PrependSlash());
            // }
            // else
            // {
            // var markup = _menuService.GetMainMenuButtons();
            // await _botClient.SendTextMessageAsync(chat, HelpFindPartner, replyMarkup: markup);
            // return;
            // }
        }

        await ExecuteCommandHandler(commandHandler, update);
    }
}
