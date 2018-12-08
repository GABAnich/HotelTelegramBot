﻿using HotelTelegramBot.Model;
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

            if (e.Message == null)
            {
                return;
            }

            var userMessage = e.Message;

            if (userMessage == null || userMessage.Type != MessageType.Text)
                return;

            // Loger придумати куда запихнути
            Console.WriteLine($"{userChat.LastName} {userChat.FirstName} {chatId}: {userInput}");

            try
            {
                await Services.AddUserChatAsync(chatId);
                await RouteMessageTextAsync(userInput, chatId, userChat);
                await RouteMessageChatPositionAsync(Services.GetChatPosition(chatId), userInput, chatId, userChat);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            string userInput = e.CallbackQuery.Data;
            long chatId = e.CallbackQuery.Message.Chat.Id;
            Chat userChat = e.CallbackQuery.Message.Chat;

            await Program.botClient.EditMessageTextAsync(
                chatId: e.CallbackQuery.Message.Chat.Id,
                messageId: e.CallbackQuery.Message.MessageId,
                text: $"*{e.CallbackQuery.Data}*",
                parseMode: ParseMode.Markdown
                );

            await RouteMessageTextAsync(userInput, chatId, userChat);
            await RouteMessageChatPositionAsync(Services.GetChatPosition(chatId), userInput, chatId, userChat);
        }

        public static async Task RouteMessageTextAsync(string userInput, long chatId, Chat userChat)
        {
            if (userInput == "/start")
            {
                await Services.ChangePositionAsync(chatId, "/start");
            }
            else if (userInput == "🎛 Головне меню")
            {
                await Services.ChangePositionAsync(chatId, "🎛 Головне меню");
            }
            else if (userInput == "🏨 Замовити номер")
            {
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 0");
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                await Services.ChangePositionAsync(chatId, "❌ Зняти бронювання 0");
            }
        }

        private static async Task RouteMessageChatPositionAsync(string chatPosition, string userInput, long chatId, Chat userChat)
        {
            if (chatPosition == "/start")
            {
                await Services.ClearUserTempDataAsync(chatId);
                await SendPhotoAsync(userChat, Services.GetImageAboutHotel(), Services.GetInfoAboutHotel(), Keyboards.MainKeyboard);
            }
            else if (chatPosition == "🎛 Головне меню")
            {
                await Services.ClearUserTempDataAsync(chatId);
                await SendMessageAsync(userChat, "Виберіть пунк меню", Keyboards.MainKeyboard);
            }
            else if (chatPosition == "❌ Зняти бронювання 0")
            {
                await SendMessageAsync(userChat, "Бронювання 1");
                await SendMessageAsync(userChat, "Бронювання 2");
                await SendMessageAsync(userChat, "Бронювання 3");
                await Services.ChangePositionAsync(chatId, "❌ Зняти бронювання 1");
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                await SendMessageAsync(userChat, "Знаття бронювання...");
                await SendMessageAsync(userChat, "Бронювання знято", Keyboards.ReturnMainMenu);
                await Services.ChangePositionAsync(chatId, "/start");
            }
            else if (chatPosition == "🏨 Замовити номер 0")
            {
                DateTime firstDate = DateTime.Now.AddDays(1);
                DateTime secondDate = firstDate.AddDays(6);
                List<string> dates = Services.GetIntermediateDates(firstDate, secondDate);
                await SendMessageAsync(userChat, "Введіть дату прибуття", Keyboards.NextDates(dates));
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 1");
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
                await Services.SaveUserTempDataAsync("DateOfArrival", userInput, chatId);

                DateTime firstDate = DateTime.Parse(userInput).AddDays(1);
                DateTime secondDate = firstDate.AddDays(6);
                List<string> dates = Services.GetIntermediateDates(firstDate, secondDate);

                await SendMessageAsync(userChat, "Введіть дату відбуття", Keyboards.NextDates(dates));
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 2");
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
                    Services.GetUserTempData(chatId, "DateOfArrival"),
                    userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateRange);
                    return;
                }
                await Services.SaveUserTempDataAsync("DateOfDeparture", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть кількість дорослих", Keyboards.Adults);
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 3");
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
                await Services.SaveUserTempDataAsync("NumberOfAdults", userInput, chatId);
                await SendMessageAsync(userChat, "Введіть кількість дітей", Keyboards.Children);
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 4");
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
                await Services.SaveUserTempDataAsync("NumberOfChildren", userInput, chatId);
                var listRoomTypes = Services.GetAviableRoomTypes(userChat);

                if (listRoomTypes.Count > 0)
                {
                    List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
                    foreach (HotelRoomType t in listRoomTypes)
                    {
                        keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData($"Замовити: {t.Name}", t.Name)
                    });
                    }

                    IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

                    await SendMessageAsync(userChat, "Оберіть тип номеру", markup);
                }
                else
                {
                    await SendMessageAsync(userChat, "На вказаний період немає доступних номерів.");
                }

                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 5");
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                await Services.SaveUserTempDataAsync("HotelRoomTypeName", userInput, chatId);
                await SendMessageAsync(userChat, "Прізвище", Keyboards.Text(userChat.LastName));
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 6");
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                await Services.SaveUserTempDataAsync("SecondName", userInput, chatId);
                await SendMessageAsync(userChat, "Ім’я", Keyboards.Text(userChat.FirstName));
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 7");
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                await Services.SaveUserTempDataAsync("FirstName", userInput, chatId);
                await SendMessageAsync(userChat, "По батькові");
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 8");
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                await Services.SaveUserTempDataAsync("MiddleName", userInput, chatId);
                await SendMessageAsync(userChat, "Номер телефону");
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 9");
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                if (!Validator.CheckPhoneNumber(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadPhoneNumber);
                    return;
                }
                await Services.SaveUserTempDataAsync("Number", userInput, chatId);
                await SendMessageAsync(userChat, "Email");
                await Services.ChangePositionAsync(chatId, "🏨 Замовити номер 10");
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                if (!Validator.CheckEmail(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadEmail);
                    return;
                }
                await Services.SaveUserTempDataAsync("Email", userInput, chatId);
                await SendMessageAsync(userChat, "Очікування бронювання");
                await Services.AddReservationAsync(chatId);
                await SendMessageAsync(userChat, "Бронювання відбулось успішно", Keyboards.ReturnMainMenu);
                await Services.ChangePositionAsync(chatId, "/start");
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
