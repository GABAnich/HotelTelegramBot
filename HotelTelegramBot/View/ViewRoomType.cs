﻿using HotelTelegramBot.Model;

namespace HotelTelegramBot.View
{
    class ViewRoomType
    {
        internal static string GetTextAboutRoomType(HotelRoomType roomType)
        {
            return "" +
                $"*{roomType.Name}*\n\n" +
                $"{roomType.Description}\n\n" +
                $"*Площа:* {roomType.Area} м^2\n" +
                $"*Послуги:* {roomType.Services}\n\n" +
                $"*Ціна за ніч:* {roomType.Price} грн";
        }
    }
}
