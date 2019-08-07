using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class MessageController
    {
        private static List<Tuple<long, ChatResponder>> chatResponders = new List<Tuple<long, ChatResponder>>();

        internal static void OnMessageAsync(object sender, MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            SetUpChatResponder(e, chat);
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            Chat chat = e.CallbackQuery.Message.Chat;
            int messageId = e.CallbackQuery.Message.MessageId;
            await Program.botClient.DeleteMessageAsync(chat, messageId);
            SetUpChatResponder(e, chat);
        }

        private static void SetUpChatResponder(EventArgs e, Chat chat)
        {
            if (!chatResponders.Exists(c => c.Item1 == chat.Id))
            {
                chatResponders.Add(new Tuple<long, ChatResponder>(chat.Id, new ChatResponder(chat, new Start())));
                return;
            }

            chatResponders.FirstOrDefault(c => c.Item1 == chat.Id).Item2.ReceiveMessageAsync(e);
        }
    }
}
