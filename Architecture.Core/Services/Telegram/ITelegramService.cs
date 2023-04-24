using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Architecture.Core.Services.Telegram
{
    public interface ITelegramService
    {
        public Task GetMessageFromTelegram();
        public Task StartCommand(TelegramBotClient bot, Update update);
        public Task HelpCommand(TelegramBotClient bot, Update update);
        public Task GetUserCommand(TelegramBotClient bot, Update update);
        public Task DefaultCommand(TelegramBotClient bot, Update update);
    }
}
