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
                if (!Validator.CheckDateFormat(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadDateFormat);
                    return;
                }
                else if (!Validator.CheckDateBiigerCurrent(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadDateLessCurrent);
                    return;
                }
                else if (!Validator.CheckDateRange(
                    DbServices.GetUserTempDataValue(chatId, "DateOfArrival"),
                    userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadDateRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("DateOfDeparture", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть кількість дорослих", Keyboards.Adults);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 3");
            }
            else if (chatPosition == "🏨 Замовити номер 3")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                else if (!Validator.CheckNumberRange(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadNumberRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("NumberOfAdults", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть кількість дітей", Keyboards.Children);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 4");
            }
            else if (chatPosition == "🏨 Замовити номер 4")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                else if (!Validator.CheckNumberRange(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadNumberRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("NumberOfChildren", userInput, chatId);
                var listRoomTypes = DbServices.GetAviableRoomTypes(userChat);

                if (listRoomTypes.Count <= 0)
                {
                    await ServicesMessageController.SendMessageAsync(userChat, "На вказаний період немає доступних номерів.", Keyboards.ReturnMainMenu);
                }

                // do somtehing
                List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
                foreach (HotelRoomType t in listRoomTypes)
                {
                    keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData($"Замовити: {t.Name}", $"{t.Id}")
                    });
                }
                IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

                await ServicesMessageController.SendMessageAsync(userChat, "Оберіть тип номеру", markup);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 5");
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                long id = long.Parse(userInput);

                if (ServicesHotelRoomType.GetHotelRoomTypeById(id) == null || !DbServices.GetAviableRoomTypes(userChat).Exists(t => t.Id == id))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, "Оберіть тип номеру", Keyboards.ReturnMainMenu);
                    return;
                };

                await DbServices.SaveUserTempDataAsync("HotelRoomTypeId", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть прізвище", Keyboards.Text(userChat.LastName));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 6");
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                if (!Validator.CheckName(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("SecondName", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть ім’я", Keyboards.Text(userChat.FirstName));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 7");
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                if (!Validator.CheckName(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("FirstName", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть по батькові");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 8");
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                if (!Validator.CheckName(userInput))
                {
                    await ServicesMessageController.SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("MiddleName", userInput, chatId);
                await ServicesMessageController.SendMessageAsync(userChat, "Введіть номер телефону");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 9");
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
