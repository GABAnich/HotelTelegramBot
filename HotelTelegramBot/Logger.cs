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
            System.IO.File.AppendAllText(@"..\..\..\messages.log", text);
            Console.WriteLine(text);
        }
    }
}
