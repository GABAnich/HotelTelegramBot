using System;
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
    }
}
