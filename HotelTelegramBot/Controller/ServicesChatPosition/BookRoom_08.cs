using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_08(Chat chat, string userInput)
        {
            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("MiddleName", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть номер телефону");
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 9");
        }
    }
}
