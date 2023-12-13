namespace CryptoCoinsParser.Application.TelegramBot;

public static class CommandHandlerResolver
{
    private static readonly Dictionary<string, Type> CommandMap = new();

    private static readonly Func<string, string>[] CommandKeyResolvers =
    {
        // command name
        key => key,

        // sub command
        key => key.Split(Command).Last().Split(SubCommand).First(),

        // admin command
        key => key.Split(Command).First()
    };

    static CommandHandlerResolver()
    {
        // var menuCommandHandlers = new[]
        // {
        // };

        AddMenuCommandHandlers(CommandMap, new List<CustomBotCommand>());

        var commandHandlers = new[]
        {
            DeleteMe,
            Start
            // CreateBroadcastMessage,
            // ReNewMenu,
            // BanUser,
        };

        AddCommandHandlers(CommandMap, commandHandlers);

        var callbackCommands = new[]
        {
            GoToMenuCallback
        };

        AddCallbackCommandHandlers(CommandMap, callbackCommands);
    }

    private static void AddMenuCommandHandlers(IDictionary<string, Type> map, IEnumerable<CustomBotCommand> commands)
    {
        foreach (var command in commands)
        {
            map.Add(command.Command.PrependSlash(), command.Handler);
            map.Add(command.Description, command.Handler);
        }
    }

    private static void AddCommandHandlers(IDictionary<string, Type> map, IEnumerable<CustomBotCommand> commands)
    {
        foreach (var command in commands)
        {
            map.Add(command.Command.PrependSlash(), command.Handler);
        }
    }

    private static void AddCallbackCommandHandlers(IDictionary<string, Type> map, CustomBotCommand[] commands)
    {
        foreach (var command in commands)
        {
            map.Add(command.Command, command.Handler);
        }
    }

    public static IBotCommandHandler ResolveCommandHandler(this IServiceProvider serviceProvider, string commandName)
    {
        foreach (var resolver in CommandKeyResolvers)
        {
            if (CommandMap.TryGetValue(resolver(commandName), out var commandHandlerType))
            {
                return serviceProvider.GetService(commandHandlerType) as IBotCommandHandler;
            }
        }

        return null;
    }
}
