using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot
{
    class Logger
    {
        internal static void Log(string chatPosition, EventArgs e)
        {
            Message message;
            string messageText;

            if ((e as MessageEventArgs) != null)
            {
                message = (e as MessageEventArgs).Message;
                messageText = message.Text;
            }
            else if ((e as CallbackQueryEventArgs) != null)
            {
                message = (e as CallbackQueryEventArgs).CallbackQuery.Message;
                messageText = (e as CallbackQueryEventArgs).CallbackQuery.Data;
            }
            else
            {
                return;
            }

            string text = "" +
                    $"{message.Date.ToShortDateString()} " +
                    $"{message.Date.ToShortTimeString()} | " +
                    $"{chatPosition} | " +
                    $"{message.Chat.Id} | " +
                    $"{message.Chat.Username} |" +
                    $"{message.Chat.LastName} " +
                    $"{message.Chat.FirstName} : " +
                    $"{messageText}\n";
            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, text);
            Console.WriteLine(text);
        }

        internal static void Log(string text)
        {
            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, text);
            Console.WriteLine(text);
        }
    }
}
