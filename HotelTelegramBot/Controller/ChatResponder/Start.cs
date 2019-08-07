using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class Start : ChatState
    {
        public override async void OnCreateAsync(Chat chat)
        {
            await DbServices.ClearUserTempDataAsync(chat.Id);
            await ServicesMessageController.SendPhotoAsync(
                chat,
                AboutHotel.ImageAboutHotel,
                AboutHotel.InfoAboutHotel,
                Keyboards.MainKeyboard);
        }

        public override void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;

            if (userInput == "🏨 Замовити номер")
            {
                responder.SetState(new BookRoom_00());
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                responder.SetState(new CancelReservation_0());
            }
            else if (userInput == "⛺️ Номери")
            {
                responder.SetState(new HotelRoom_0());
            }
        }

        public override void Back()
        {
            return;
        }
    }
}
