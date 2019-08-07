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
        public static MessageController messageController;

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            botClient = new TelegramBotClient(ConfigTelegramBot.APIToken);
            messageController = new MessageController();

            botClient.OnMessage += messageController.OnMessageAsync;
            botClient.OnCallbackQuery += messageController.OnCallbackQueryAsync;

            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
    }
}
