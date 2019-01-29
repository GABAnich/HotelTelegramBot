﻿using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_06(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            string userInput = e.Message.Text;
            await BookRoom_06(chat, userInput);
        }

        private static async Task BookRoom_06(Chat chat, string userInput)
        {
            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("SecondName", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть ім’я", Keyboards.Text(chat.FirstName));
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 7");
        }
    }
}
