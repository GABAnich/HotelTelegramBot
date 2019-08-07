using HotelTelegramBot.Model;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_09 : ChatState
    {
        public BookRoom_09(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_08(chat));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

            if (!Validator.CheckEmail(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadEmail);
                return;
            }
            await DbServices.SaveUserTempDataAsync("Email", userInput, chat.Id);
            responder.SetState(new BookRoom_10(chat));
        }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть Email");
        }
    }
}