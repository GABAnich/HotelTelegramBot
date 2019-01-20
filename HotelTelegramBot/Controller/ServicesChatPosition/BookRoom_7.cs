using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_7(Chat chat, string userInput)
        {
            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("FirstName", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть по батькові");
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 8");
        }
    }
}
