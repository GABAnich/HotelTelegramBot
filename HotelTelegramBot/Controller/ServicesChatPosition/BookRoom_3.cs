using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_3(Chat chat, string userInput)
        {
            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }
            else if (!Validator.CheckNumberRange(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumberRange);
                return;
            }
            await DbServices.SaveUserTempDataAsync("NumberOfAdults", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть кількість дітей", Keyboards.Children);
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 4");
        }
    }
}
