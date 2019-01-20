using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    class MessageController
    {
        internal static async void OnMessageAsync(object sender, MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            long chatId = e.Message.Chat.Id;
            Chat userChat = e.Message.Chat;
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
                await DbServices.CrateIfNotExistUserChatAsync(chatId);
                await RouteMessageTextAsync(userInput, chatId, userChat);

                chatPosition = DbServices.GetChatPositionByIdChat(chatId);

                string text = "" +
                    $"{e.Message.Date.ToShortDateString()} " +
                    $"{e.Message.Date.ToShortTimeString()} | " +
                    $"{chatPosition} | " +
                    $"{chatId} | " +
                    $"{userChat.Username} |" +
                    $"{userChat.LastName} " +
                    $"{userChat.FirstName} : " +
                    $"{userInput}\n";
                System.IO.File.AppendAllText(@"..\..\..\messages.log", text);
                Console.WriteLine(text);

                await RouteMessageChatPositionAsync(chatPosition, userInput, chatId, userChat);
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
            await RouteMessageTextAsync(userInput, chatId, userChat);
            await RouteMessageChatPositionAsync(DbServices.GetChatPositionByIdChat(chatId), userInput, chatId, userChat);
        }

        public static async Task RouteMessageTextAsync(string userInput, long chatId, Chat userChat)
        {
            if (userInput == "/start")
            {
                await DbServices.ChangePositionAsync(chatId, "/start");
            }
            else if (userInput == "🎛 Головне меню")
            {
                await DbServices.ChangePositionAsync(chatId, "🎛 Головне меню");
            }
            else if (userInput == "🏨 Замовити номер")
            {
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 0");
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                await DbServices.ChangePositionAsync(chatId, "❌ Зняти бронювання 0");
            }
            else if (userInput == "⛺️ Номери")
            {
                await DbServices.ChangePositionAsync(chatId, "⛺️ Номери 0");
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
                await ServicesChatPosition.BookRoom_0(userChat);
            }
            else if (chatPosition == "🏨 Замовити номер 1")
            {
                await ServicesChatPosition.BookRoom_1(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 2")
            {
                await ServicesChatPosition.BookRoom_2(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 3")
            {
                await ServicesChatPosition.BookRoom_3(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 4")
            {
                await ServicesChatPosition.BookRoom_4(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                await ServicesChatPosition.BookRoom_5(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                await ServicesChatPosition.BookRoom_6(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                await ServicesChatPosition.BookRoom_7(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                await ServicesChatPosition.BookRoom_8(userChat, userInput);
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                if (!Validator.CheckPhoneNumber(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadPhoneNumber);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("Number", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть Email");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 10");
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                if (!Validator.CheckEmail(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadEmail);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("Email", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Очікування бронювання");
                Reservation r = await DbServices.AddReservationAsync(chatId);
                HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
                HotelRoomType t = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
                int countDays = DbServices.GetIntermediateDates(r.DateOfArrival, r.DateOfArrival).Count;
                string text = "" +
                    $"*{t.Name}\n*" +
                    $"\n" +
                    $"Прізвище: {r.SecondName}\n" +
                    $"Ім’я: {r.FirstName}\n" +
                    $"По батькові: {r.MiddleName}\n" +
                    $"Номер телефону: {r.Number}\n" +
                    $"Email: {r.Email}\n" +
                    $"Період: {r.DateOfArrival}-{r.DateOfDeparture}\n" +
                    $"Дорослих: {r.NumberOfAdults}\n" +
                    $"Дітей: {r.NumberOfChildren}\n" +
                    $"\n" +
                    $"Кімната: {room.Name}\n" +
                    $"Поверх: {room.Floor}\n" +
                    $"\n" +
                    $"До оплати: {countDays * t.Price} грн\n" +
                    $"\n" +
                    $"Ідентифікатор для перевірки: *494ebf5f419ad02a86af25f8db5ed114790399c2aa6b233384b1b4b9ac3458e5*";
                await ServicesMessageController.SendMessageAsync(userChat, text, Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chatId, "/start");
            }
        }
    }
}
