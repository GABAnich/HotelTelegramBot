using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_10(Chat chat, string userInput)
        {
            if (!Validator.CheckEmail(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadEmail);
                return;
            }
            await DbServices.SaveUserTempDataAsync("Email", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Очікування бронювання");
            Reservation r = await DbServices.AddReservationAsync(chat.Id);
            HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
            HotelRoomType t = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
            int countDays = DbServices.GetIntermediateDates(r.DateOfArrival, r.DateOfArrival).Count;
            string text = ViewReservation.GetTextAboutReservation(r, t, room, countDays);
            await ServicesMessageController.SendMessageAsync(chat, text, Keyboards.ReturnMainMenu);
            await DbServices.ChangePositionAsync(chat.Id, "/start");
        }
    }
}
