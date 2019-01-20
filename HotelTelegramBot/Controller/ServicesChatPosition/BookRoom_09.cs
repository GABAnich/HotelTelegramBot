using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_09(Chat chat, string userInput)
        {
            if (!Validator.CheckPhoneNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadPhoneNumber);
                return;
            }
            await DbServices.SaveUserTempDataAsync("Number", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть Email");
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 10");
        }
    }
}
