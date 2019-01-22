using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_05(Chat chat, string userInput)
        {
            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }
            long id = long.Parse(userInput);

            if (ServicesHotelRoomType.GetHotelRoomTypeById(id) == null || 
                !DbServices.GetAviableRoomTypes(chat).Exists(t => t.Id == id))
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "Оберіть тип номеру", Keyboards.ReturnMainMenu);
                return;
            };

            await DbServices.SaveUserTempDataAsync("HotelRoomTypeId", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть прізвище", Keyboards.Text(chat.LastName));
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 6");
        }
    }
}
