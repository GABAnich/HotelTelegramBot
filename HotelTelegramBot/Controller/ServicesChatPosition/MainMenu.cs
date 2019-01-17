using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task MainMenuAsync(Chat chat)
        {
            await DbServices.ClearUserTempDataAsync(chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Виберіть пунк меню", Keyboards.MainKeyboard);
        }
    }
}
