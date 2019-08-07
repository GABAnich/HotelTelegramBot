using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class Start : ChatState
    {
        public Start(Chat chat) : base(chat) { }

        protected override async void OnCreateAsync()
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

            if (userInput == "🏨 Замовити номер")
            {
                responder.SetState(new BookRoom_00(chat));
            }
            else if (userInput == "❌ Зняти бронювання")
            {
                responder.SetState(new CancelReservation_0(chat));
            }
            else if (userInput == "⛺️ Номери")
            {
                responder.SetState(new HotelRoom_0(chat));
            }
        }

        public override void Back()
        {
            return;
        }
    }
}
