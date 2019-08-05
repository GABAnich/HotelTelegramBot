using HotelTelegramBot.Model;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_07 : ChatState
    {
        public BookRoom_07(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_06(chat));
        }

        public override async Task ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("MiddleName", userInput, chat.Id);
            responder.SetState(new BookRoom_08(chat));
        }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть по батькові");
        }
    }
}