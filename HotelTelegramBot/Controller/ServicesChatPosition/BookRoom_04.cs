﻿using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_04(Chat chat, string userInput)
        {
            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }
            else if (!Validator.CheckNumberRange(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumberRange);
                return;
            }
            await DbServices.SaveUserTempDataAsync("NumberOfChildren", userInput, chat.Id);
            var listRoomTypes = DbServices.GetAviableRoomTypes(chat);

            if (listRoomTypes.Count <= 0)
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "На вказаний період немає доступних номерів.", Keyboards.ReturnMainMenu);
            }

            // do somtehing
            List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
            foreach (HotelRoomType t in listRoomTypes)
            {
                keyboards.Add(new List<InlineKeyboardButton>() {
                        InlineKeyboardButton.WithCallbackData($"Замовити: {t.Name}", $"{t.Id}")
                    });
            }
            IReplyMarkup markup = new InlineKeyboardMarkup(keyboards);

            await ServicesMessageController.SendMessageAsync(chat, "Оберіть тип номеру", markup);
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 5");
        }
    }
}
