# Crypto Coins Parser Telegram Bot

## Development

1. ### [Install pre-commit hooks](https://pre-commit.com/)

2. ### Start external dependencies by executing ```docker-compose up```

## EF Core Migrations 

```
cd src/CryptoCoinsParser.Api
dotnet ef migrations add InitialCreate -c CryptoCoinsParser.Persistence.Context.TelegramBotContext -v -p ../CryptoCoinsParser.Persistence/CryptoCoinsParser.Persistence.csproj
```
