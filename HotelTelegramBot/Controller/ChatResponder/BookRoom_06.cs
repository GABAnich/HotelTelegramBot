using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_06 : ChatState
    {
        public BookRoom_06(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_05(chat));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("FirstName", userInput, chat.Id);
            responder.SetState(new BookRoom_07(chat));
        }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть ім’я",
                Keyboards.Text(chat.FirstName));
        }
    }
}