using System.Threading.Tasks;

namespace Architecture.Core.Services.Telegram
{
    public interface ITelegramService
    {
        public Task GetMessageFromTelegram();
    }
}
