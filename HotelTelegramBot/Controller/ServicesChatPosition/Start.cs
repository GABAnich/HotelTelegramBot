using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task StartAsync(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;

            await DbServices.ClearUserTempDataAsync(chat.Id);
            await ServicesMessageController.SendPhotoAsync(
                chat, 
                AboutHotel.ImageAboutHotel, 
                AboutHotel.InfoAboutHotel, 
                Keyboards.MainKeyboard);
        }
    }
}
