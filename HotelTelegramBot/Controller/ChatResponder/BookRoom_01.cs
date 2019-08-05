using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_01 : ChatState
    {
        public BookRoom_01(Chat chat) : base(chat)
        {
        }

        protected override async void OnCreateAsync()
        {
            string firstDateString = DbServices.GetUserTempDataValue(chat.Id, "DateOfArrival");
            DateTime firstDate = DateTime.Parse(firstDateString).AddDays(1);
            DateTime secondDate = firstDate.AddDays(6);
            List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);
            await ServicesMessageController.SendMessageAsync(
                chat, "Введіть дату відбуття", Keyboards.NextDates(dates));
        }

        public override async Task ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as MessageEventArgs).Message.Text;

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
            else if (!Validator.CheckDateRange(
                DbServices.GetUserTempDataValue(chat.Id, "DateOfArrival"),
                userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateRange);
                return;
            }
            await DbServices.SaveUserTempDataAsync("DateOfDeparture", userInput, chat.Id);

            responder.SetState(new BookRoom_02(chat));
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_00(chat));
        }
    }
}
