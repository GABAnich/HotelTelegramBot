using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_00 : ChatState
    {
        public override async void OnStateChange(Chat chat)
        {
            DateTime firstDate = DateTime.Now.AddDays(1);
            DateTime secondDate = firstDate.AddDays(6);
            List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть дату прибуття", Keyboards.NextDates(dates));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;

            if (!Validator.CheckDateFormat(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateFormat);
                return;
            }
            else if (!Validator.CheckDateBigerCurrent(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateLessCurrent);
                return;
            }
            await DbServices.SaveUserTempDataAsync("DateOfArrival", userInput, chat.Id);

            responder.SetState(new BookRoom_01());
        }

        public override void Back()
        {
            responder.SetState(new Start());
        }
    }
}
