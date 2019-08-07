using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    class ServicesMessageController
    {
        internal static async Task SendPhotosAsync(long chatId, List<string> photos)
        {
            List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();
            foreach (string str in photos)
            {
                inputMediaPhotos.Add(new InputMediaPhoto(str));
            }

            try
            {
                #pragma warning disable CS0618 // Type or member is obsolete
                Message[] msg = await Program.botClient.SendMediaGroupAsync(
                            chatId: chatId,
                            media: inputMediaPhotos
                        );
                #pragma warning restore CS0618 // Type or member is obsolete
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    Logger.Log(exception.Message);
                    return;
                }
            }
        }

        internal static async Task SendPhotoAsync(ChatId chatId,
            string photo,
            string caption,
            IReplyMarkup keyboard)
        {
            try
            {
                await Program.botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: photo,
                    caption: caption,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: keyboard);
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    Logger.Log(exception.Message);
                    return;
                }
            }
        }

        internal static async Task SendMessageAsync(ChatId chatId,
            string text,
            IReplyMarkup keyboard = null)
        {
            if (keyboard == null)
            {
                keyboard = new ReplyKeyboardRemove();
            }

            try
            {
                Message message = await Program.botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: text,
                        parseMode: ParseMode.Markdown,
                        disableNotification: true,
                        replyMarkup: keyboard
                    );
            }
            catch (Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    Logger.Log(exception.Message);
                    return;
                }
            }
        }
    }
}
