using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_05 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new BookRoom_04());
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;

            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("SecondName", userInput, chat.Id);
            responder.SetState(new BookRoom_06());
        }

        public override async void OnStateChange(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть прізвище", Keyboards.Text(chat.LastName));
        }
    }
}