using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class BookRoom_03 : ChatState
    {
        public BookRoom_03(Chat chat) : base(chat) { }

        protected override async void OnCreateAsync()
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть кількість дітей", Keyboards.Children);
        }

        public override async Task ReceiveMessageAsync(EventArgs e)
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
            await DbServices.SaveUserTempDataAsync("NumberOfChildren", userInput, chat.Id);
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_02(chat));
        }
    }
}
