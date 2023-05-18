using bmbr_wrhs_bot;
using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

// создание строки токена из менеджера конфигурации

string Token = ConfigurationManager.ConnectionStrings["botToken"].ConnectionString;

// создаем бот

TelegramBotClient botClient = new TelegramBotClient(Token);

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

// начинаем получение информации ботом 

botClient.StartReceiving(
    updateHandler: Services.HandleUpdateAsync,
    pollingErrorHandler: Services.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

// получаем информацию бота о самом себе

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начало работы бота @{me.Username}");
Console.ReadLine();

// Окончание работы программы

cts.Cancel();