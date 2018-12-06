using HotelTelegramBot.Model;
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
        public static async void CallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            string userInput = e.CallbackQuery.Message.Text;
            long userChatId = e.CallbackQuery.Message.Chat.Id;
            Chat userChat = e.CallbackQuery.Message.Chat;

            await Program.botClient.EditMessageTextAsync(
                chatId: e.CallbackQuery.Message.Chat.Id,
                messageId: e.CallbackQuery.Message.MessageId,
                text: $"*{e.CallbackQuery.Data}*",
                parseMode: ParseMode.Markdown
                );

            //else if (chatPosition == "🏨 Замовити номер 5")
            await Services.SaveUserTempDataAsync("HotelRoomType", userInput, userChatId);
            await SendMessageAsync(userChat, "Прізвище");
            await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 6");
        }

        async public static void MessageRouteAsync(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                string userInput = e.Message.Text;
                long userChatId = e.Message.Chat.Id;
                Chat userChat = e.Message.Chat;

                try
                {
                    var userMessage = e.Message;

                    if (userMessage == null || userMessage.Type != MessageType.Text)
                        return;

                    // Loger придумати куда запихнути
                    Console.WriteLine($"{userChat.LastName} {userChat.FirstName} {userChatId}: {userInput}");

                    await Services.AddUserChatAsync(e.Message.Chat.Id);
                    await RouteMessageTextAsync(e);
                    await RouteMessageChatPositionAsync(Services.GetChatPosition(e.Message.Chat.Id, e), e);
                }
                catch
                {
                    return;
                }
            }
        }

        public static async Task RouteMessageTextAsync(MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            long userChatId = e.Message.Chat.Id;
            Chat userChat = e.Message.Chat;

            if (userInput == "/start")
            {
                await Services.ChangePositionAsync(userChatId, "/start");
            }
            else if (userInput == "🎛 Головне меню")
            {
                await Services.ChangePositionAsync(userChatId, "🎛 Головне меню");
            }
            else if (userInput == "🏨 Замовити номер")
            {
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 0");
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                await Services.ChangePositionAsync(userChatId, "❌ Зняти бронювання 0");
            }
        }

        private static async Task RouteMessageChatPositionAsync(string chatPosition, MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            long userChatId = e.Message.Chat.Id;
            Chat userChat = e.Message.Chat;

            if (chatPosition == "/start")
            {
                await Services.ClearUserTempDataAsync(userChatId);
                await SendPhotoAsync(userChat, Services.GetImageAboutHotel(), Services.GetInfoAboutHotel(), Keyboards.MainKeyboard);
            }
            else if (chatPosition == "🎛 Головне меню")
            {
                await Services.ClearUserTempDataAsync(userChatId);
                await SendMessageAsync(userChat, "Виберіть пунк меню", Keyboards.MainKeyboard);
            }
            else if (chatPosition == "❌ Зняти бронювання 0")
            {
                await SendMessageAsync(userChat, "Бронювання 1");
                await SendMessageAsync(userChat, "Бронювання 2");
                await SendMessageAsync(userChat, "Бронювання 3");
                await Services.ChangePositionAsync(userChatId, "❌ Зняти бронювання 1");
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                await SendMessageAsync(userChat, "Знаття бронювання...");
                await SendMessageAsync(userChat, "Бронювання знято", Keyboards.ReturnMainMenu);
                await Services.ChangePositionAsync(userChatId, "/start");
            }
            else if (chatPosition == "🏨 Замовити номер 0")
            {
                await SendMessageAsync(userChat, "Введіть дату прибуття");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 1");
            }
            else if (chatPosition == "🏨 Замовити номер 1")
            {
                if (!Validator.CheckDateFormat(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateFormat);
                    return;
                }
                else if (!Validator.CheckDateLessCurrent(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateLessCurrent);
                    return;
                }
                await Services.SaveUserTempDataAsync("DateOfArrival", userInput, userChatId);
                await SendMessageAsync(userChat, "Введіть дату відбуття");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 2");
            }
            else if (chatPosition == "🏨 Замовити номер 2")
            {
                if (!Validator.CheckDateFormat(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateFormat);
                    return;
                }
                else if (!Validator.CheckDateLessCurrent(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateLessCurrent);
                    return;
                }
                else if (!Validator.CheckDateRange(
                    Services.GetUserTempData(userChatId, "DateOfArrival"),
                    userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadDateRange);
                    return;
                }
                await Services.SaveUserTempDataAsync("DateOfDeparture", userInput, userChatId);
                await SendMessageAsync(userChat, "Введіть кількість дорослих");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 3");
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
                await Services.SaveUserTempDataAsync("NumberOfAdults", userInput, userChatId);
                await SendMessageAsync(userChat, "Введіть кількість дітей");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 4");
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
                await Services.SaveUserTempDataAsync("NumberOfChildren", userInput, userChatId);
                var listRoomTypes = Services.GetAviableRoomTypes(userChat);

                List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
                foreach (HotelRoomType t in listRoomTypes)
                {
                    keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData($"Замовити: {t.Name}", t.Name)
                    });
                }

                IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

                await SendMessageAsync(userChat, "Оберіть тип номеру", markup);
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 5");
            }
            else if (chatPosition == "🏨 Замовити номер 6")
            {
                await Services.SaveUserTempDataAsync("SecondName", userInput, userChatId);
                await SendMessageAsync(userChat, "Ім’я");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 7");
            }
            else if (chatPosition == "🏨 Замовити номер 7")
            {
                await Services.SaveUserTempDataAsync("FirstName", userInput, userChatId);
                await SendMessageAsync(userChat, "По батькові");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 8");
            }
            else if (chatPosition == "🏨 Замовити номер 8")
            {
                await Services.SaveUserTempDataAsync("MiddleName", userInput, userChatId);
                await SendMessageAsync(userChat, "Номер телефону");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 9");
            }
            else if (chatPosition == "🏨 Замовити номер 9")
            {
                await Services.SaveUserTempDataAsync("Number", userInput, userChatId);
                await SendMessageAsync(userChat, "Email");
                await Services.ChangePositionAsync(userChatId, "🏨 Замовити номер 10");
            }
            else if (chatPosition == "🏨 Замовити номер 10")
            {
                if (!Validator.CheckEmail(userInput))
                {
                    await SendMessageAsync(userChat, Validator.BadEmail);
                    return;
                }
                await Services.SaveUserTempDataAsync("Email", userInput, userChatId);
                await SendMessageAsync(userChat, "Очікування бронювання");
                await SendMessageAsync(userChat, "Бронювання відбулось успішно");
                await SendMessageAsync(userChat, "Скачати файл", Keyboards.ReturnMainMenu);
                await Services.ChangePositionAsync(userChatId, "/start");
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
            InputMedia media = new InputMedia("http://panorama-hotel.com.ua/photos/gallery/IMG_78330209093803.jpg");
            List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();
            foreach(string str in photos)
            {
                inputMediaPhotos.Add(new InputMediaPhoto(str));
            }

            Message[] msg = await Program.botClient.SendMediaGroupAsync(
                    chatId: chatId,
                    media: inputMediaPhotos
                );
        }
    }
}
