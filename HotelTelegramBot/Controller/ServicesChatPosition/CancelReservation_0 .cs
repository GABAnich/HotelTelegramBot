using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task CancelReservation_0(Chat chat)
        {
            var listReservation = DbServices.GetValidReservation(chat.Id, DateTime.Now);
            if (listReservation.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Бронювань немає", Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chat.Id, "/start");
                return;
            }

            List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
            foreach (Reservation r in listReservation)
            {
                HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
                HotelRoomType roomType = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
                keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData(
                            $"{roomType.Name}: {r.DateOfArrival}-{r.DateOfDeparture}",
                            $"{r.Id}"
                        )
                    });
            }
            IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

            await ServicesMessageController.SendMessageAsync(chat, "Бронювання: ", markup);
            await DbServices.ChangePositionAsync(chat.Id, "❌ Зняти бронювання 1");
        }
    }
}
