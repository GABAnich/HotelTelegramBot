using HotelTelegramBot.Model;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_08 : ChatState
    {
        public BookRoom_08(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_07(chat));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            MessageEventArgs ev = e as MessageEventArgs;
            string userInput;

            if (ev.Message.Type == MessageType.Text)
            {
                userInput = ev.Message.Text;
            }
            else if (ev.Message.Type == MessageType.Contact)
            {
                userInput = ev.Message.Contact.PhoneNumber;
            }
            else
            {
                return;
            }

            if (!Validator.CheckPhoneNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadPhoneNumber);
                return;
            }
            await DbServices.SaveUserTempDataAsync("Number", userInput, chat.Id);
            responder.SetState(new BookRoom_09(chat));
        }

        protected override async void OnCreateAsync()
        {
            IReplyMarkup markup = View.Keyboards.GetRequestcontactKeyboard("📞 Мій номер");
            await ServicesMessageController.SendMessageAsync(chat, "Введіть номер телефону", markup);
        }
    }
}