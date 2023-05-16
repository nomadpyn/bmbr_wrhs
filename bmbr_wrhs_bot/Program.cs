using bmbr_wrhs_bot;
using System.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

string Token = ConfigurationManager.ConnectionStrings["botToken"].ConnectionString;

TelegramBotClient botClient = new TelegramBotClient(Token);

using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: Services.HandleUpdateAsync,
    pollingErrorHandler: Services.HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Начало работы бота @{me.Username}");
Console.ReadLine();

cts.Cancel();