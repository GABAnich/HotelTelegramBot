﻿using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HotelTelegramBot.Controller
{
    class MessageController
    {
        internal static async void OnMessageAsync(object sender, MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            Chat chat = e.Message.Chat;
            string chatPosition;

            if (e.Message == null)
            {
                return;
            }

            if (e.Message.Type != MessageType.Text && e.Message.Type != MessageType.Contact)
            {
                return;
            }

            try
            {
                await DbServices.CrateIfNotExistUserChatAsync(chat.Id);
                await RouteMenuAsync(userInput, chat);

                chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
                Logger.Log(chatPosition, e);
                await RouteMessageChatPositionAsync(chatPosition, e);
            }
            catch(Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    Logger.Log("Forbidden: bot was blocked by the user");
                    return;
                }
            }
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            Chat chat = e.CallbackQuery.Message.Chat;
            int messageId = e.CallbackQuery.Message.MessageId;
            string chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
            string userInput = e.CallbackQuery.Data;

            Logger.Log(chatPosition, e);

            await Program.botClient.DeleteMessageAsync(chat, messageId);
            await RouteMenuAsync(userInput, chat);
            await RouteMessageChatPositionAsync(chatPosition, e);
        }

        private static async Task RouteMenuAsync(string userInput, Chat chat)
        {
            if (userInput == "/start")
            {
                await DbServices.ChangePositionAsync(chat.Id, "/start");
            }
            else if (userInput == "🎛 Головне меню")
            {
                await DbServices.ChangePositionAsync(chat.Id, "🎛 Головне меню");
            }
            else if (userInput == "🏨 Замовити номер")
            {
                await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 0");
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                await DbServices.ChangePositionAsync(chat.Id, "❌ Зняти бронювання 0");
            }
            else if (userInput == "⛺️ Номери")
            {
                await DbServices.ChangePositionAsync(chat.Id, "⛺️ Номери 0");
            }
        }

        private static async Task RouteMessageChatPositionAsync(string chatPosition, MessageEventArgs e)
        {
            if (chatPosition == "/start")
            {
                await ServicesChatPosition.StartAsync(e);
            }
            else if (chatPosition == "🎛 Головне меню")
            {
                await ServicesChatPosition.MainMenuAsync(e);
            }
            else if (chatPosition == "⛺️ Номери 0")
            {
                await ServicesChatPosition.HotelRoom_0(e);
            }
            else if (chatPosition == "❌ Зняти бронювання 0")
            {
                await ServicesChatPosition.CancelReservation_0(e);
            }
            else if (chatPosition == "🏨 Замовити номер 0")
            {
                await ServicesChatPosition.BookRoom_00(e);
            }
            else if (chatPosition == "🏨 Замовити номер 1")
            {
                await ServicesChatPosition.BookRoom_01(e);
            }
            else if (chatPosition == "🏨 Замовити номер 2")
            {
                await ServicesChatPosition.BookRoom_02(e);
            }
            else if (chatPosition == "🏨 Замовити номер 3")
            {
                await ServicesChatPosition.BookRoom_03(e);
            }
            else if (chatPosition == "🏨 Замовити номер 4")
            {
                await ServicesChatPosition.BookRoom_04(e);
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                await ServicesChatPosition.BookRoom_06(e);
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                await ServicesChatPosition.BookRoom_07(e);
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                await ServicesChatPosition.BookRoom_08(e);
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                await ServicesChatPosition.BookRoom_09(e);
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                await ServicesChatPosition.BookRoom_10(e);
            }
        }

        private static async Task RouteMessageChatPositionAsync(string chatPosition, CallbackQueryEventArgs e)
        {
            if (chatPosition == "⛺️ Номери 1")
            {
                await ServicesChatPosition.HotelRoom_1(e);
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                await ServicesChatPosition.CancelReservation_1(e);
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                await ServicesChatPosition.BookRoom_05(e);
            }
        }
    }
}
