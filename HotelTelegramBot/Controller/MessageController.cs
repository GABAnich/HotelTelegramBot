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

            if (!chatResponders.Exists(c => c.Item1 == chat.Id))
            {
                chatResponders.Add(new Tuple<long, ChatResponder>(chat.Id, new ChatResponder(chat, new Start())));
                return;
            }

            chatResponders.FirstOrDefault(c => c.Item1 == chat.Id).Item2.ReceiveMessageAsync(e);

            //string userInput = e.Message.Text;
            //Chat chat = e.Message.Chat;
            //string chatPosition;

            //try
            //{
            //    await DbServices.CrateIfNotExistUserChatAsync(chat.Id);
            //    await ServicesMessageController.RouteMenuAsync(userInput, chat);

            //    chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
            //    Logger.Log(chatPosition, e);
            //    await ServicesMessageController.RouteMessageChatPositionAsync(chatPosition, e);
            //}
            //catch (Telegram.Bot.Exceptions.ApiRequestException exception)
            //{
            //    if (exception.Message == "Forbidden: bot was blocked by the user")
            //    {
            //        Logger.Log(exception.Message);
            //        return;
            //    }
            //}
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            Chat chat = e.CallbackQuery.Message.Chat;
            int messageId = e.CallbackQuery.Message.MessageId;

            if (!chatResponders.Exists(c => c.Item1 == chat.Id))
            {
                chatResponders.Add(new Tuple<long, ChatResponder>(chat.Id, new ChatResponder(chat, new Start())));
                return;
            }

            await Program.botClient.DeleteMessageAsync(chat, messageId);
            chatResponders.FirstOrDefault(c => c.Item1 == chat.Id).Item2.ReceiveMessageAsync(e);

            //Chat chat = e.CallbackQuery.Message.Chat;
            //int messageId = e.CallbackQuery.Message.MessageId;
            //string chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
            //string userInput = e.CallbackQuery.Data;

            //Logger.Log(chatPosition, e);

            //await Program.botClient.DeleteMessageAsync(chat, messageId);
            //await ServicesMessageController.RouteMenuAsync(userInput, chat);
            //await ServicesMessageController.RouteMessageChatPositionAsync(chatPosition, e);
        }
    }
}
