using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task CancelReservation_1(CallbackQueryEventArgs e)
        {
            Chat chat = e.CallbackQuery.Message.Chat;
            string userInput = e.CallbackQuery.Data;
            await CancelReservation_1(chat, userInput);
        }

        private static async Task CancelReservation_1(Chat chat, string userInput)
        {
            Reservation r = ServicesReservation.GetReservationById(int.Parse(userInput));
            if (r == null)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Виберіть бронювання із списку", Keyboards.MainKeyboard);
            }
            await ServicesMessageController.SendMessageAsync(chat, "Знаття бронювання...");
            await DbServices.DeleteHotelRoomReservedDateByRoomIdAsync(r.Id);
            await ServicesReservation.DeleteReservationByIdAsync(r.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Бронювання знято", Keyboards.ReturnMainMenu);
            await DbServices.ChangePositionAsync(chat.Id, "/start");
        }
    }
}
