using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    internal class CancelReservation_0 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new MainMenu());
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as CallbackQueryEventArgs).CallbackQuery.Data;
            Chat chat = (e as CallbackQueryEventArgs).CallbackQuery.Message.Chat;

            Reservation r = ServicesReservation.GetReservationById(int.Parse(userInput));
            if (r == null)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Виберіть бронювання із списку", Keyboards.MainKeyboard);
            }
            await ServicesMessageController.SendMessageAsync(chat, "Знаття бронювання...");
            await DbServices.DeleteHotelRoomReservedDateByRoomIdAsync(r.Id);
            await ServicesReservation.DeleteReservationByIdAsync(r.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Бронювання знято", Keyboards.ReturnMainMenu);

            responder.SetState(new MainMenu());
        }

        public override async void OnCreateAsync(Chat chat)
        {
            var listReservation = DbServices.GetValidReservation(chat.Id, DateTime.Now);
            if (listReservation.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Бронювань немає", Keyboards.ReturnMainMenu);
                responder.SetState(new MainMenu());
                return;
            }
            IReplyMarkup markup = Keyboards.GetReservationsMenu(listReservation);

            await ServicesMessageController.SendMessageAsync(chat, "Бронювання: ", markup);
        }
    }
}