using HotelTelegramBot.Model;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_08 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new BookRoom_07());
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            MessageEventArgs ev = e as MessageEventArgs;
            Chat chat = ev.Message.Chat;
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
            //await DbServices.SaveUserTempDataAsync("Number", userInput, chat.Id);
            responder.userTempData.Add("Number", userInput);
            responder.SetState(new BookRoom_09());
        }

        public override async void OnStateChange(Chat chat)
        {
            IReplyMarkup markup = View.Keyboards.GetRequestcontactKeyboard("📞 Мій номер");
            await ServicesMessageController.SendMessageAsync(chat, "Введіть номер телефону", markup);
        }
    }
}