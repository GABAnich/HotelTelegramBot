﻿using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task HotelRoom_0(Chat chat)
        {
            List<HotelRoomType> listRoomTypes = ServicesHotelRoomType.GetHotelRoomTypes();

            if (listRoomTypes.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Номерів немає", Keyboards.ReturnMainMenu);
                return;
            }

            // Remake this. Should be at least one method in View. Maybe two methods.
            List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
            foreach (HotelRoomType t in listRoomTypes)
            {
                keyboards.Add(new List<InlineKeyboardButton>() {
                    InlineKeyboardButton.WithCallbackData($"{t.Name}", $"{t.Id}")
                });
            }
            IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

            await ServicesMessageController.SendMessageAsync(chat, "Оберіть тип номеру", markup);
            await DbServices.ChangePositionAsync(chat.Id, "⛺️ Номери 1");

        }
    }
}
