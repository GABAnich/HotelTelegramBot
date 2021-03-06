﻿using HotelTelegramBot.View;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class MainMenu : Start
    {
        public override async void OnStateChange(Chat chat)
        {
            responder.userTempData.Clear();
            await ServicesMessageController.SendMessageAsync(chat.Id, "Головне меню", Keyboards.MainKeyboard);
        }

        public override void Back()
        {
            responder.SetState(new Start());
        }
    }
}
