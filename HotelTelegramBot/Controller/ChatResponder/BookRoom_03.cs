using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class BookRoom_03 : ChatState
    {
        public override async void OnStateChange(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть кількість дітей", Keyboards.Children);
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;

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
            responder.userTempData["NumberOfChildren"] = userInput;
            responder.SetState(new BookRoom_04());
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_02());
        }
    }
}
