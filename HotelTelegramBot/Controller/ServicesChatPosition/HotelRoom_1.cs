﻿using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task HotelRoom_1(Chat chat, string userInput)
        {
            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }

            long roomTypeId = int.Parse(userInput);
            HotelRoomType roomType = ServicesHotelRoomType.GetHotelRoomTypeById(roomTypeId);
            List<string> photos = DbServices.GetHotelRoomTypeImagesUrl(roomTypeId);

            if (roomType == null)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Такого типу номеру не існує", Keyboards.ReturnMainMenu);
                return;
            }

            await ServicesMessageController.SendPhotosAsync(chat.Id, photos);

            string message = "" +
                $"*{roomType.Name}*\n\n" +
                $"{roomType.Description}\n\n" +
                $"*Площа:* {roomType.Area} м^2\n" +
                $"*Послуги:* {roomType.Services}\n\n" +
                $"*Ціна за ніч:* {roomType.Price} грн";
            await ServicesMessageController.SendMessageAsync(chat, message, Keyboards.ReturnMainMenu);
            await DbServices.ChangePositionAsync(chat.Id, "/start");
        }
    }
}