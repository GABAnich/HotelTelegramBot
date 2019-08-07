using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_10 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new BookRoom_09());
        }

        public override void ReceiveMessageAsync(EventArgs e)
        {
            responder.SetState(new MainMenu());
        }

        public override async void OnCreateAsync(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(chat, "Очікування бронювання");

            Reservation r = await DbServices.AddReservationAsync(chat.Id);
            HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
            HotelRoomType t = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
            int countDays = DbServices.GetIntermediateDates(r.DateOfArrival, r.DateOfArrival).Count;

            string text = ViewReservation.GetTextAboutReservation(r, t, room, countDays);
            await ServicesMessageController.SendMessageAsync(chat, text, Keyboards.ReturnMainMenu);
        }
    }
}