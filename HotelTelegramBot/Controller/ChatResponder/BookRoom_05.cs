using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_05 : ChatState
    {
        public BookRoom_05(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_04(chat));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("SecondName", userInput, chat.Id);
            responder.SetState(new BookRoom_06(chat));
        }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть прізвище", Keyboards.Text(chat.LastName));
        }
    }
}