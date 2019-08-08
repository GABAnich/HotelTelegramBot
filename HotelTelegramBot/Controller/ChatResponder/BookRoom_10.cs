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

        public override async void OnStateChange(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(chat, "Очікування бронювання");

            responder.userTempData.TryGetValue("HotelRoomTypeId", out string hotelRoomTypeId);
            responder.userTempData.TryGetValue("DateOfArrival", out string arrival);
            responder.userTempData.TryGetValue("DateOfDeparture", out string departure);
            responder.userTempData.TryGetValue("SecondName", out string secondName);
            responder.userTempData.TryGetValue("FirstName", out string firstName);
            responder.userTempData.TryGetValue("MiddleName", out string middleName);
            responder.userTempData.TryGetValue("Number", out string number);
            responder.userTempData.TryGetValue("Email", out string email);
            responder.userTempData.TryGetValue("NumberOfAdults", out string adults);
            responder.userTempData.TryGetValue("NumberOfChildren", out string children);

            Reservation reservation = new Reservation()
            {
                IdUserChat = chat.Id,
                SecondName = secondName,
                FirstName = firstName,
                MiddleName = middleName,
                Number = number,
                Email = email,
                DateOfArrival = arrival,
                DateOfDeparture = departure,
                NumberOfAdults = int.Parse(adults),
                NumberOfChildren = int.Parse(children)
            };

            Reservation r = await DbServices.AddReservationAsync(chat.Id, int.Parse(hotelRoomTypeId), reservation);
            HotelRoom room = ServicesHotelRoom.GetHotelRoomById(r.HotelRoomId);
            HotelRoomType t = ServicesHotelRoomType.GetHotelRoomTypeById(room.HotelRoomTypeId);
            int countDays = DbServices.GetIntermediateDates(r.DateOfArrival, r.DateOfArrival).Count;

            string text = ViewReservation.GetTextAboutReservation(r, t, room, countDays);
            await ServicesMessageController.SendMessageAsync(chat, text, Keyboards.ReturnMainMenu);
        }
    }
}