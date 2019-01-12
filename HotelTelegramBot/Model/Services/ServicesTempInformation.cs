﻿using System.Threading.Tasks;

namespace HotelTelegramBot.Model.Services
{
    class ServicesTempInformation
    {
        public static async Task AddTempInformationAsync(long chatId, string property, string value)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var obj = new TempInformation()
                {
                    IdUserChat = chatId,
                    Property = property,
                    Value = value
                };
                db.TempInformation.Add(obj);

                await db.SaveChangesAsync();
            }
        }
    }
}
