using Newtonsoft.Json;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Logger
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

            LogSchema obj = new LogSchema()
            {
                Date = message.Date.ToShortDateString(),
                Time = message.Date.ToShortTimeString(),
                ChatPosition = chatPosition,
                ChatId = message.Chat.Id,
                Username = message.Chat.Username,
                LastName = message.Chat.LastName,
                FirstName = message.Chat.FirstName,
                MessageText = messageText
            };
            string json = JsonConvert.SerializeObject(obj);

            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, json);
            Console.WriteLine(json);
        }

        internal static void Log(string text)
        {
            var obj = new
            {
                Text = text
            };
            string json = JsonConvert.SerializeObject(obj);

            System.IO.File.AppendAllText(ConfigTelegramBot.LogFile, json);
            Console.WriteLine(json);
        }
    }
}
