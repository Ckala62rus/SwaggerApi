using Architecture.Core.Services.Users;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Architecture.Core.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        public TelegramService(IUsersService usersService)
        {
            _usersService = usersService;
        }

        private const string baseUrl = "https://api.telegram.org/bot";
        private const string token = "token token ";
        private const string method = "/getUpdates";

        private readonly IUsersService _usersService;

        public async Task GetMessageFromTelegram()
        {
            var user = await _usersService.GetUser(1);

            WebClient wc = new WebClient() { Encoding = Encoding.UTF8 };

            int update_id = 0;

            while (true)
            {
                try
                {
                    Thread.Sleep(1000);

                    var r = wc.DownloadString($"{baseUrl + token + method }?offset={update_id}");

                    var msgs = JObject.Parse(r)["result"].ToArray();

                    foreach (dynamic msg in msgs)
                    {
                        update_id = Convert.ToInt32(msg.update_id) + 1;

                        string message = msg.message.text;
                        string userId = msg.message.from.id;

                        SendMessage(userId, message);

                        if (message == "/start")
                        {
                            Console.WriteLine(message);
                            SendMessage(userId, message);
                        }
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        
        public void SendMessage(string chatId, string text)
        {
            var client = new HttpClient();
            client.GetAsync($"{baseUrl}{token}/sendMessage?chat_id={chatId}&text={text}").Wait();
        }
    }
}
