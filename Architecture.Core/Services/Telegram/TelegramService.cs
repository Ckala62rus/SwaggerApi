using Architecture.Core.Services.Users;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Architecture.Core.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        public TelegramService(
            IUsersService usersService,
            IConfiguration configuration
        ) {
            _usersService = usersService;
            _configuration = configuration;
        }

        private const string baseUrl = "https://api.telegram.org/bot";
        private const string token = "token token ";
        private const string method = "/getUpdates";
        private int offset = 0;

        private readonly IUsersService _usersService;
        private readonly IConfiguration _configuration;

        public void SendMessage(string chatId, string text)
        {
            var client = new HttpClient();
            client.GetAsync($"{baseUrl}{token}/sendMessage?chat_id={chatId}&text={text}").Wait();
        }

        public async Task GetMessageFromTelegram()
        {
            var token = _configuration["Telegram:Token"];
            TelegramBotClient bot = new TelegramBotClient(token);

            await bot.SetWebhookAsync("");
            int timeout = 100;

            while (true)
            {
                try
                {
                    var updates = await bot.GetUpdatesAsync(offset, timeout);

                    foreach (var update in updates)
                    {
                        //if (update.Message?.Contact != null)
                        //{
                        //    Console.WriteLine(update.Message?.Contact?.PhoneNumber);
                        //    continue;
                        //}

                        switch (update.Type)
                        {
                            case UpdateType.CallbackQuery:
                                var chatId = update.CallbackQuery.Message.Chat.Id;
                                var pressedButtonID = update.CallbackQuery.Data; // Сюда вытягиваешь callbackData из кнопки.

                                var text = update.CallbackQuery.Message.Text;
                                Console.WriteLine($"Pressed button = {pressedButtonID}");

                                await bot.SendTextMessageAsync(chatId, $"Вы выбрали: {pressedButtonID}");
                                break;
                            default:
                                break;
                        }

                        if (update.Message == null)
                        {
                            offset = update.Id + 1;
                            continue;
                        }

                        var message = update.Message.Text;

                        if (message == "/start")
                        {
                            await StartCommand(bot, update);
                        }
                        else if (message == "/help")
                        {
                            await HelpCommand(bot, update);
                        }
                        else if((Regex.Matches(message, "^/user-(\\d{1,9})$")).Count > 0) // /user-1
                        {
                            await GetUserCommand(bot, update);
                        }
                        else
                        {
                            await DefaultCommand(bot, update);
                        }

                        offset = update.Id + 1;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public async Task StartCommand(TelegramBotClient bot, Update update)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = 
            new(
                new[]
                    {
                        new KeyboardButton[] { "/help" },
                        new KeyboardButton[]{ KeyboardButton.WithRequestContact("Контакт") },
                    }
            ) {
                ResizeKeyboard = true
            };

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Да, я пойду на встречу!", callbackData: "yes"),
                    InlineKeyboardButton.WithCallbackData(text: "Нет, не пойду", callbackData: "no"),
                },
            });

            await bot.SendTextMessageAsync(
                update.Message.From.Id,
                "Привет! Я бот для разработки.",
                replyMarkup: replyKeyboardMarkup
            );
        }

        public async Task HelpCommand(TelegramBotClient bot, Update update)
        {
            string msg =
                "*****Help***** \n" +
                "Доступные команды \n" +
                "/help - Общая информация \n" +
                "/user-1 - Показать пользователя, где 'id' уникальный идентификатор \n";

            await bot.SendTextMessageAsync(update.Message.From.Id, msg);
        }

        public async Task DefaultCommand(TelegramBotClient bot, Update update)
        {
            await bot.SendTextMessageAsync(update.Message.From.Id, $"Я не понимаю Вашу команду {update.Message.Text}");
        }

        public async Task GetUserCommand(TelegramBotClient bot, Update update)
        {
            MatchCollection matches = Regex.Matches(update.Message.Text, "^/user-(\\d{1,2})");
            foreach (Match match in matches)
            {
                var id = int.Parse(match.Groups[1].Value);

                var user = await _usersService.GetUser(id);

                if (user != null)
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, $"Ещем пользователя с идентификатором: {id}");
                    await bot.SendTextMessageAsync(update.Message.From.Id, $"Электронная почта {user.Email}");
                }
                else
                {
                    await bot.SendTextMessageAsync(update.Message.From.Id, $"Пользователь с идентификатором: {id} не найден.");
                }
            }
        }
    }
}
