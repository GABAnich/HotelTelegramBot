using System;
using Telegram.Bot.Args;

namespace HotelTelegramBot
{
    class Logger
    {
        public static void Log(string chatPosition, MessageEventArgs e)
        {
            string text = "" +
                    $"{e.Message.Date.ToShortDateString()} " +
                    $"{e.Message.Date.ToShortTimeString()} | " +
                    $"{chatPosition} | " +
                    $"{e.Message.Chat.Id} | " +
                    $"{e.Message.Chat.Username} |" +
                    $"{e.Message.Chat.LastName} " +
                    $"{e.Message.Chat.FirstName} : " +
                    $"{e.Message.Text}\n";
            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, text);
            Console.WriteLine(text);
        }

        public static void Log(string chatPosition, CallbackQueryEventArgs e)
        {
            string text = "" +
                    $"{e.CallbackQuery.Message.Date.ToShortDateString()} " +
                    $"{e.CallbackQuery.Message.Date.ToShortTimeString()} | " +
                    $"{chatPosition} | " +
                    $"{e.CallbackQuery.Message.Chat.Id} | " +
                    $"{e.CallbackQuery.Message.Chat.Username} |" +
                    $"{e.CallbackQuery.Message.Chat.LastName} " +
                    $"{e.CallbackQuery.Message.Chat.FirstName} : " +
                    $"{e.CallbackQuery.Data}\n";
            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, text);
            Console.WriteLine(text);
        }
    }
}
