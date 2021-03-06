﻿using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_09 : ChatState
    {
        public override void Back()
        {
            responder.SetState(new BookRoom_08());
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;

            if (!Validator.CheckEmail(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadEmail);
                return;
            }
            responder.userTempData["Email"] = userInput;
            responder.SetState(new BookRoom_10());
        }

        public override async void OnStateChange(Chat chat)
        {
            await ServicesMessageController.SendMessageAsync(chat, "Введіть Email");
        }
    }
}