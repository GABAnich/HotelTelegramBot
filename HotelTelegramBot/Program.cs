using HotelTelegramBot.Controller;
using System;
using System.Text;
using System.Threading;
using Telegram.Bot;

namespace HotelTelegramBot
{
    class Program
    {
        public static ITelegramBotClient botClient;

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            botClient = new TelegramBotClient(ConfigTelegramBot.GetAPIToken());

            botClient.OnMessage += MessageController.OnMessageAsync;
            botClient.OnCallbackQuery += MessageController.OnCallbackQueryAsync;

            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
    }
}
