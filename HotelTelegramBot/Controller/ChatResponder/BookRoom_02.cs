using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_02 : ChatState
    {
        public BookRoom_02(Chat chat) : base(chat)
        {
        }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть кількість дорослих", Keyboards.Adults);
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }
            else if (!Validator.CheckNumberRange(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumberRange);
                return;
            }
            await DbServices.SaveUserTempDataAsync("NumberOfAdults", userInput, chat.Id);

            responder.SetState(new BookRoom_03(chat));
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_01(chat));
        }
    }
}