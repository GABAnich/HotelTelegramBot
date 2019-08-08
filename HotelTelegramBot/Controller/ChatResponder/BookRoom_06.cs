using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_06 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new BookRoom_05());
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
            //await DbServices.SaveUserTempDataAsync("FirstName", userInput, chat.Id);
            responder.userTempData.Add("FirstName", userInput);
            responder.SetState(new BookRoom_07());
        }

        public override async void OnStateChange(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть ім’я",
                Keyboards.Text(chat.FirstName));
        }
    }
}