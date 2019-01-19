using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_1(Chat chat, string userInput)
        {
            if (!Validator.CheckDateFormat(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateFormat);
                return;
            }
            else if (!Validator.CheckDateBiigerCurrent(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateLessCurrent);
                return;
            }
            await DbServices.SaveUserTempDataAsync("DateOfArrival", userInput, chat.Id);

            DateTime firstDate = DateTime.Parse(userInput).AddDays(1);
            DateTime secondDate = firstDate.AddDays(6);
            List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);

            await ServicesMessageController.SendMessageAsync(chat, "Введіть дату відбуття", Keyboards.NextDates(dates));
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 2");
        }
    }
}
