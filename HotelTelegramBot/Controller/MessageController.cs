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
                await DbServices.ClearUserTempDataAsync(chatId);
                await SendPhotoAsync(userChat, AboutHotel.ImageAboutHotel, AboutHotel.InfoAboutHotel, Keyboards.MainKeyboard);
            }
            else if (chatPosition == "🎛 Головне меню")
            {
                await DbServices.ClearUserTempDataAsync(chatId);
                await SendMessageAsync(userChat, "Виберіть пунк меню", Keyboards.MainKeyboard);
            }
            else if (chatPosition == "⛺️ Номери 0")
            {
                List<HotelRoomType> listRoomTypes = ServicesHotelRoomType.GetRoomTypes();

                if (listRoomTypes.Count == 0)
                {
                    await SendMessageAsync(userChat, "Номерів немає", Keyboards.ReturnMainMenu);
                    return;
                }

                List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
                foreach (HotelRoomType t in listRoomTypes)
                {
                    keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData($"{t.Name}", $"{t.Id}")
                    });
                }
                IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

                await SendMessageAsync(userChat, "Оберіть тип номеру", markup);
                await DbServices.ChangePositionAsync(chatId, "⛺️ Номери 1");
            }
            else if (chatPosition == "⛺️ Номери 1")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }

                long roomTypeId = int.Parse(userInput);
                HotelRoomType roomType = ServicesHotelRoomType.GetHotelRoomTypeById(roomTypeId);
                List<string> photos = DbServices.GetHotelRoomTypeImagesUrl(roomTypeId);

                if (roomType == null)
                {
                    await SendMessageAsync(userChat, "Такого типу номеру не існує", Keyboards.ReturnMainMenu);
                    return;
                }

                await SendPhotosAsync(chatId, photos);

                string message = "" +
                    $"*{roomType.Name}*\n\n" +
                    $"{roomType.Description}\n\n" +
                    $"*Площа:* {roomType.Area} м^2\n" +
                    $"*Послуги:* {roomType.Services}\n\n" +
                    $"*Ціна за ніч:* {roomType.Price} грн";
                await SendMessageAsync(userChat, message, Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chatId, "/start");
            }
            else if (chatPosition == "❌ Зняти бронювання 0")
            {
                var listReservation = DbServices.GetValidReservation(chatId, DateTime.Now);
                if (listReservation.Count == 0)
                {
                    await SendMessageAsync(userChat, "Бронювань немає", Keyboards.ReturnMainMenu);
                    await DbServices.ChangePositionAsync(chatId, "/start");
                    return;
                }

                List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
                foreach (Reservation r in listReservation)
                {
                    HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
                    HotelRoomType roomType = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
                    keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData(
                            $"{roomType.Name}: {r.DateOfArrival}-{r.DateOfDeparture}",
                            $"{r.Id}"
                        )
                    });
                }
                IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

                await SendMessageAsync(userChat, "Бронювання: ", markup);
                await DbServices.ChangePositionAsync(chatId, "❌ Зняти бронювання 1");
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                Reservation r = ServicesReservation.GetReservationById(int.Parse(userInput));
                if (r == null)
                {
                    await SendMessageAsync(userChat, "Виберіть бронювання із списку", Keyboards.MainKeyboard);
                }
                await SendMessageAsync(userChat, "Знаття бронювання...");
                // ERROR DOWN
                await DbServices.DeleteHotelRoomReservedDateByRoomIdAsync(r.Id);
                await ServicesReservation.DeleteReservationByIdAsync(r.Id);
                // ERROR UP
                await SendMessageAsync(userChat, "Бронювання знято", Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chatId, "/start");
            }
            else if (chatPosition == "🏨 Замовити номер 0")
            {
                DateTime firstDate = DateTime.Now.AddDays(1);
                DateTime secondDate = firstDate.AddDays(6);
                List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);
                await SendMessageAsync(userChat, "Введіть дату прибуття", Keyboards.NextDates(dates));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 1");
            }
            else if (chatPosition == "🏨 Замовити номер 1")
            {
                if (!Validator.CheckDateFormat(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateFormat);
                    return;
                }
                else if (!Validator.CheckDateBiigerCurrent(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateLessCurrent);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("DateOfArrival", userInput, chatId);

                DateTime firstDate = DateTime.Parse(userInput).AddDays(1);
                DateTime secondDate = firstDate.AddDays(6);
                List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);

                await SendMessageAsync(userChat, "Введіть дату відбуття", Keyboards.NextDates(dates));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 2");
            }
            else if (chatPosition == "🏨 Замовити номер 2")
            {
                if (!Validator.CheckDateFormat(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateFormat);
                    return;
                }
                else if (!Validator.CheckDateBiigerCurrent(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateLessCurrent);
                    return;
                }
                else if (!Validator.CheckDateRange(
                    DbServices.GetUserTempDataValue(chatId, "DateOfArrival"),
                    userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("DateOfDeparture", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть кількість дорослих", Keyboards.Adults);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 3");
            }
            else if (chatPosition == "🏨 Замовити номер 3")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                else if (!Validator.CheckNumberRange(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumberRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("NumberOfAdults", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть кількість дітей", Keyboards.Children);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 4");
            }
            else if (chatPosition == "🏨 Замовити номер 4")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                else if (!Validator.CheckNumberRange(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumberRange);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("NumberOfChildren", userInput, chatId);
                var listRoomTypes = DbServices.GetAviableRoomTypes(userChat);

                if (listRoomTypes.Count <= 0)
                {
                    await SendMessageAsync(userChat, "На вказаний період немає доступних номерів.", Keyboards.ReturnMainMenu);
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

                await SendMessageAsync(userChat, "Оберіть тип номеру", markup);
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 5");
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                if (!Validator.CheckNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadNumber);
                    return;
                }
                long id = long.Parse(userInput);

                if (ServicesHotelRoomType.GetHotelRoomTypeById(id) == null || !DbServices.GetAviableRoomTypes(userChat).Exists(t => t.Id == id))
                {
                    await SendMessageAsync(userChat, "Оберіть тип номеру", Keyboards.ReturnMainMenu);
                    return;
                };

                await DbServices.SaveUserTempDataAsync("HotelRoomTypeId", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть прізвище", Keyboards.Text(userChat.LastName));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 6");
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                if (!Validator.CheckName(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("SecondName", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть ім’я", Keyboards.Text(userChat.FirstName));
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 7");
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                if (!Validator.CheckName(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("FirstName", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть по батькові");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 8");
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                if (!Validator.CheckName(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadName);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("MiddleName", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть номер телефону");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 9");
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                if (!Validator.CheckPhoneNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadPhoneNumber);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("Number", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть Email");
                await DbServices.ChangePositionAsync(chatId, "🏨 Замовити номер 10");
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                if (!Validator.CheckEmail(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadEmail);
                    return;
                }
                await DbServices.SaveUserTempDataAsync("Email", userInput, chatId);
                await SendMessageAsync(userChat, "Очікування бронювання");
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
                await SendMessageAsync(userChat, text, Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chatId, "/start");
            }
        }

        private static async Task SendMessageAsync(ChatId chatId,
            string text,
            IReplyMarkup keyboard = null)
        {
            if (keyboard == null)
            {
                keyboard = new ReplyKeyboardRemove();
            }

            Message message = await Program.botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: text,
                    parseMode: ParseMode.Markdown,
                    disableNotification: true,
                    replyMarkup: keyboard
                );
        }

        private static async Task SendPhotoAsync(ChatId chatId,
            string photo,
            string caption,
            IReplyMarkup keyboard)
        {
            await Program.botClient.SendPhotoAsync(
                chatId: chatId,
                photo: photo,
                caption: caption,
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard);
        }

        private static async Task SendPhotosAsync(ChatId chatId,
            List<string> photos)
        {
            List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();
            foreach(string str in photos)
            {
                inputMediaPhotos.Add(new InputMediaPhoto(str));
            }

            #pragma warning disable CS0618 // 'ITelegramBotClient.SendMediaGroupAsync(ChatId, IEnumerable<InputMediaBase>, bool, int, CancellationToken)' is obsolete: 'Use the other overload of this method instead. Only photo and video input types are allowed.'
            Message[] msg = await Program.botClient.SendMediaGroupAsync(
                    chatId: chatId,
                    media: inputMediaPhotos
                );
            #pragma warning restore CS0618 // 'ITelegramBotClient.SendMediaGroupAsync(ChatId, IEnumerable<InputMediaBase>, bool, int, CancellationToken)' is obsolete: 'Use the other overload of this method instead. Only photo and video input types are allowed.'
        }
    }
}
