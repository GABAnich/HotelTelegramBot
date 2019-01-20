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
            string text = "" +
                $"*{t.Name}\n*" +
                $"\n" +
                $"Прізвище: {r.SecondName}\n" +
                $"Ім’я: {r.FirstName}\n" +
                $"По батькові: {r.MiddleName}\n" +
                $"Номер телефону: {r.Number}\n" +
                $"Email: {r.Email}\n" +
                $"Період: {r.DateOfArrival}-{r.DateOfDeparture}\n" +
                $"Дорослих: {r.NumberOfAdults}\n" +
                $"Дітей: {r.NumberOfChildren}\n" +
                $"\n" +
                $"Кімната: {room.Name}\n" +
                $"Поверх: {room.Floor}\n" +
                $"\n" +
                $"До оплати: {countDays * t.Price} грн\n" +
                $"\n" +
                $"Ідентифікатор для перевірки: *494ebf5f419ad02a86af25f8db5ed114790399c2aa6b233384b1b4b9ac3458e5*";
            await ServicesMessageController.SendMessageAsync(chat, text, Keyboards.ReturnMainMenu);
            await DbServices.ChangePositionAsync(chat.Id, "/start");
        }
    }
}
