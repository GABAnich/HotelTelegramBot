using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

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
    }
}
