using System.Collections.Generic;
using System.Threading.Tasks;
using HotelTelegramBot.Model;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    class ServicesMessageController
    {
        internal static async Task RouteMenuAsync(string userInput, Chat chat)
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

        internal static async Task RouteMessageChatPositionAsync(string chatPosition, MessageEventArgs e)
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

        internal static async Task RouteMessageChatPositionAsync(string chatPosition, CallbackQueryEventArgs e)
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

        internal static async Task SendPhotosAsync(long chatId, List<string> photos)
        {
            List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();
            foreach (string str in photos)
            {
                inputMediaPhotos.Add(new InputMediaPhoto(str));
            }

            #pragma warning disable CS0618 // Type or member is obsolete
            Message[] msg = await Program.botClient.SendMediaGroupAsync(
                    chatId: chatId,
                    media: inputMediaPhotos
                );
            #pragma warning restore CS0618 // Type or member is obsolete
        }

        internal static async Task SendPhotoAsync(ChatId chatId,
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

        internal static async Task SendMessageAsync(ChatId chatId,
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
    }
}
