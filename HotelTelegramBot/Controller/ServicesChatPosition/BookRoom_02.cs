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
        internal static async Task BookRoom_02(Chat chat, string userInput)
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
            else if (!Validator.CheckDateRange(
                DbServices.GetUserTempDataValue(chat.Id, "DateOfArrival"),
                userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadDateRange);
                return;
            }
            await DbServices.SaveUserTempDataAsync("DateOfDeparture", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть кількість дорослих", Keyboards.Adults);
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 3");
        }
    }
}
