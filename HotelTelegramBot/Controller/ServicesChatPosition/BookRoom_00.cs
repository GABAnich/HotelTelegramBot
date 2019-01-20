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
        internal static async Task BookRoom_00(Chat chat)
        {
            DateTime firstDate = DateTime.Now.AddDays(1);
            DateTime secondDate = firstDate.AddDays(6);
            List<string> dates = DbServices.GetIntermediateDates(firstDate, secondDate);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть дату прибуття", Keyboards.NextDates(dates));
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 1");
        }
    }
}
