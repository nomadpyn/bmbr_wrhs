using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using bmbr_wrhs;

namespace bmbr_wrhs_bot
{
    // статический класс для работы бота
    public static class Services
    {
        // поля для хранения информации о брендах, разрешенных слова и разрешенных пользователях

        static string[] brands;
        static List<string> allowedWords;
        static long[] allowedId;
        static txtLogger myLogger;

        // коллекция для сборки запроса в базу данных

        static Dictionary<long, List<string>> data = new Dictionary<long, List<string>>();
        
        // конструктор по умолчанию

        static Services()
        {
            myLogger = new();
            loadStartBrands();
            loadAllowedId();
            loadAllowedWords();            
        }

        // бот получает обновления в чате от пользователя

        static internal async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {            
            if (update.Message is not { } message)
                return;            
            if (message.Text is not { } messageText)
                return;            

            long chatId = message.Chat.Id;

            // если запрещенный пользователь пытался получить доступ к боту, то выходит сообщение, в противном случае продолжается работа

            if(!allowedId.Contains(chatId))
            {
                Task<Message> action = SendNotAllowedMessage(botClient, message, cancellationToken);
                Message sentMessage = await action;
                logBotMessage($"{chatId} пробовал получить доступ к боту");
            }
            else 
            {            
            string model = await ReturnWordModel(message);
            
            logBotMessage($"Получил сообщение '{messageText}' в чате {chatId}.");

                // запуск методов в зависимости от того, какой текст пришел боту

                switch (messageText)
                {
                    case "/start":
                        {
                            Task<Message> action = SendStartMessage(botClient, message, cancellationToken);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                    case "Склад":
                        {
                            Task<Message> action = SendStartKeyboard(botClient, message, cancellationToken);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                    case "Очистить":
                        {
                            Task<Message> action = ClearData(botClient, message.Chat.Id, cancellationToken);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                    case "Getlast":
                        {
                            Task<Message> action = GetMyLastMessage(botClient, message, cancellationToken);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                    case string value when value == model:
                        {
                            Task<Message> action = SendModelKeyboard(botClient, message, cancellationToken, value);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                    default:
                        {
                            Task<Message> action = SendHelpMessage(botClient, message, cancellationToken);
                            Message sentMessage = await action;
                            logBotMessage($"Сообщение отправлено с Id: {sentMessage.MessageId}");
                            break;
                        }
                }
            };
        }

        // метод обработки ошибки API

        static internal Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Ошибка API Telegram:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            logBotMessage(ErrorMessage);
            return Task.CompletedTask;
        }

        // Стартовое сообщения для пользователя 

        static async Task<Message> SendStartMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string text = "Добро пожаловать в чат бот Склад бамперов\nЧтобы начать работу напишите \"Cклад\"";
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken);
        }

        // Сообщение о запрете работы с ботом

        static async Task<Message> SendNotAllowedMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string text = "Доступ запрещен";
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken);
        }

        // Создание стартовой ReplyKeyboard для пользователя с марками ТС

        static async Task<Message> SendStartKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {            
            List<KeyboardButton[]> buttons = new();       
            
            // создание кнопок в зависимости от четного или нечетного количества брендов 

            for (int i=0; i < brands.Length;)
            {
                if(i !=brands.Length-1)
                    buttons.Add(new KeyboardButton[] { brands[i] , brands[i+1] });
                else
                    buttons.Add(new KeyboardButton[] { brands[brands.Length - 1] });
                i += 2;
            };
            
            // создание клавиатуры из массива кнопок

            ReplyKeyboardMarkup replyKeyboardMarkup = new(buttons.ToArray())
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Марки Авто",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }

        // Создание клавиатур, для показа пользователю в иерархическом порядке

        static async Task<Message> SendModelKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken, string inbox)
        {
            List<string> search = null;
            List<string> cars = GetClass.getModelsByName(inbox);
            List<string> colors = GetClass.getCarColor(inbox);
            List<string> parts = GetClass.getPartsTypes();

            // создание клавиатуры, в зависимости от того, где в какой коллекции содержится приходящее сообщения от пользователя

            if (brands.Contains(inbox))
            {
                search = cars;
            }
            else
                if (cars.Contains(inbox))
                {
                    search = colors;
                }
                else
                    if (!parts.Contains(data[message.Chat.Id].Last()))
                    {
                        search = parts;
                    }
                    else            
                    {
                    search = null;
                    }

            // если search пустой, следовательно выдаем ответ на полный запрос пользователю, в противном случае идет следующая клавиатура по иерархии поиска

            if (search != null)
            {
                List<KeyboardButton[]> buttons = new();
                for(int i=0; i< search.Count; i++)
                {
                    string name = search[i].ToString();
                    buttons.Add(new KeyboardButton[] { name });
                }
                ReplyKeyboardMarkup replyKeyboardMarkup = new(buttons.ToArray())                    
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
                return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Выбор для {inbox}",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
            }
            else
            {
                return await getFullPart(message.Chat.Id, botClient, cancellationToken);
            }
        }

        // возвращает пользователю искомую запчасть, либо отказ, либо ошибку в случае некорректного заполнения коллекции поиска Dictionary 'data'

        static async Task<Message> getFullPart(long chatId,ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            string[]? part = data[chatId].TakeLast(3).ToArray();
            int l = part.Length;

            // возвращает строку ошибки, в случае, если нет элементов в коллекции для создания запроса в БД

            if (l < 3)
            {
                return await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "Ошибка поиска, попробуй снова",
                cancellationToken: cancellationToken);
            }
            
            // создаем запрос в БД, для поиска

            AutoPart? carSearch = GetClass.getPartFromBot(part[2], part[0], part[1]);
            string output;

            // возврат сообщения в случае отсутствия такой запчасти в БД

            if (carSearch == null)
            {
                output = "Нет такой запчасти";
                logBotMessage($"Пользователь {chatId} получил пустой вывод.");
                myLogger.log($"Пользователь {chatId} получил пустой вывод.");
            }

            // собирает строку о наличии и цене запчасти, в случае нахождения ее в БД

            else
            {
                output = carSearch.ToString();
                logBotMessage($"Пользователь {chatId} получил вывод {output}.");
            }
            await ClearData(botClient, chatId, cancellationToken);
            return await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: output,
                cancellationToken: cancellationToken);
        }
        
        // получение последнего элемента в коллекции поиска для пользователя, который запросил метод

        static async Task<Message> GetMyLastMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string text = "Для такого пользователя нет списка сообщений";
            if (data.ContainsKey(message.Chat.Id))
            {
                List<string>? listText = data[message.Chat.Id];
                
                if (listText.Count > 0)
                {
                    text = listText.Last();
                }
                else
                {
                    text = "Нет сообщений";
                }
            }
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        // очистка коллекции поиска для конкретного пользователя

        static async Task<Message> ClearData(ITelegramBotClient botClient, long chatId, CancellationToken cancellationToken)
        {
            string text = "Для такого пользователя нет списка поиска";
            if (data.ContainsKey(chatId))
            {
                data[chatId].Clear();
                text = "Список поиска очищен";
            }
            return await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        // отправка навигационного сообщения

        static async Task<Message> SendHelpMessage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            string text = "Для запуска работы используйте команду \"Склад\"\n" +
                "Для того чтобы очистить очередь поиска команда \"Очистить\"\n" +
                "Для выбора запчасти следуйте выпадающиму меню.";
            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: text,
                cancellationToken: cancellationToken);

        }

        // возвращение строки в основном метод работы бота, и добавление его в коллекцию при прохождении условий отбора

        static async Task<string> ReturnWordModel(Message message)
        {
            long chatId = message.Chat.Id;
            string model = null;
            if (data.ContainsKey(chatId))
            {
                if (allowedWords.Contains(message.Text))
                {
                    data[chatId].Add(message.Text);

                    model = data[chatId].Last();
                };
            }
            else
            {
                if (allowedWords.Contains(message.Text))
                {
                    data.Add(chatId, new List<string>() { message.Text });
                    model = data[chatId].Last();
                };
            }

            return model;
        }

        // загрузка из БД, слов для работы бота 

        static void loadAllowedWords()
        {
            allowedWords = GetClass.allDataInString();
            allowedWords.AddRange(brands);
        }

        // загрузка из csv файла id пользователей, допущенных к работе

        static void loadAllowedId()
        {
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader("bmbrIdAllowed.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString();
                        string[] rows = Fulltext.Split('\n');
                        allowedId = new long[rows.Length - 1];
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            allowedId[i] = Convert.ToInt64(rows[i]);
                        }
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                logBotMessage(ex.Message + "\nПрограмма будет закрыта");
                Environment.Exit(0);
            }            
        }

        // загрузка из csv данных для стартовой клавиатуры
       
        static void loadStartBrands()
        {
            string Fulltext;
            try
            {
                using (StreamReader sr = new StreamReader("startBrands.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString();
                        string[] rows = Fulltext.Split('\n');
                        brands = new string[rows.Length - 1];
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            brands[i] = rows[i].Trim();
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                logBotMessage(ex.Message + "\nПрограмма будет закрыта");
                Environment.Exit(0);
            }
        }

        static void logBotMessage(string message)
        {
            Console.WriteLine(message);
            myLogger.log(message);
        }
    }
}