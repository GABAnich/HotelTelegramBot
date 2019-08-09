using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_01 : ChatState
    {
        public override async void OnStateChange(Chat chat)
        {
            responder.userTempData.TryGetValue("DateOfArrival", out string firstDateString);
            DateTime firstDate = DateTime.Parse(firstDateString).AddDays(1);
            DateTime secondDate = firstDate.AddDays(6);
            List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть дату відбуття", Keyboards.NextDates(dates));
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;
            Chat chat = (e as MessageEventArgs).Message.Chat;
            responder.userTempData.TryGetValue("DateOfArrival", out string dateOfArrival);

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
            else if (!Validator.CheckDateRange(dateOfArrival, userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateRange);
                return;
            }
            responder.userTempData["DateOfDeparture"] = userInput;
            responder.SetState(new BookRoom_02());
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_00());
        }
    }
}
