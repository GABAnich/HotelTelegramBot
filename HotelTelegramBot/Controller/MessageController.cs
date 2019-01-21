using HotelTelegramBot.Model;
using System;
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

            var userMessage = e.Message;

            if (userMessage == null || userMessage.Type != MessageType.Text)
                return;

            try
            {
                await DbServices.CrateIfNotExistUserChatAsync(chat.Id);
                await RouteMessageTextAsync(userInput, chat);

                chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);

                string text = "" +
                    $"{e.Message.Date.ToShortDateString()} " +
                    $"{e.Message.Date.ToShortTimeString()} | " +
                    $"{chatPosition} | " +
                    $"{chat.Id} | " +
                    $"{chat.Username} |" +
                    $"{chat.LastName} " +
                    $"{chat.FirstName} : " +
                    $"{userInput}\n";
                System.IO.File.AppendAllText(@"..\..\..\messages.log", text);
                Console.WriteLine(text);

                await RouteMessageChatPositionAsync(chatPosition, userInput, chat.Id, chat);
            }
            catch(Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    return;
                }
            }
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            string userInput = e.CallbackQuery.Data;
            long chatId = e.CallbackQuery.Message.Chat.Id;
            Chat userChat = e.CallbackQuery.Message.Chat;

            await Program.botClient.DeleteMessageAsync(e.CallbackQuery.Message.Chat, e.CallbackQuery.Message.MessageId);
            await RouteMessageTextAsync(userInput, userChat);
            await RouteMessageChatPositionAsync(DbServices.GetChatPositionByIdChat(chatId), userInput, chatId, userChat);
        }

        public static async Task RouteMessageTextAsync(string userInput, Chat chat)
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

        private static async Task RouteMessageChatPositionAsync(string chatPosition, string userInput, long chatId, Chat userChat)
        {
            if (chatPosition == "/start")
            {
                await ServicesChatPosition.StartAsync(userChat);
            }
            else if (chatPosition == "🎛 Головне меню")
            {
                await ServicesChatPosition.MainMenuAsync(userChat);
            }
            else if (chatPosition == "⛺️ Номери 0")
            {
                await ServicesChatPosition.HotelRoom_0(userChat);
            }
            else if (chatPosition == "⛺️ Номери 1")
            {
                await ServicesChatPosition.HotelRoom_1(userChat, userInput);
            }
            else if (chatPosition == "❌ Зняти бронювання 0")
            {
                await ServicesChatPosition.CancelReservation_0(userChat);
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                await ServicesChatPosition.CancelReservation_1(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 0")
            {
                await ServicesChatPosition.BookRoom_00(userChat);
            }
            else if (chatPosition == "🏨 Замовити номер 1")
            {
                await ServicesChatPosition.BookRoom_01(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 2")
            {
                await ServicesChatPosition.BookRoom_02(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 3")
            {
                await ServicesChatPosition.BookRoom_03(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 4")
            {
                await ServicesChatPosition.BookRoom_04(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                await ServicesChatPosition.BookRoom_05(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                await ServicesChatPosition.BookRoom_06(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                await ServicesChatPosition.BookRoom_07(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                await ServicesChatPosition.BookRoom_08(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                await ServicesChatPosition.BookRoom_09(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                await ServicesChatPosition.BookRoom_10(userChat, userInput);
            }
        }
    }
}
