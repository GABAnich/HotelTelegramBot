﻿using System.Linq;
using System.Threading.Tasks;

namespace HotelTelegramBot.Model.Services
{
    class ServicesUserChat
    {
        public static UserChat GetUserChatByIdChat(long idChat)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.UserChats
                    .Where(u => u.IdChat == idChat)
                    .FirstOrDefault();
            }
        }

        public static string GetChatPositionByIdChat(long idChat)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == idChat)
                    .FirstOrDefault();

                return userChat.ChatPosition;
            }
        }

        public static async Task AddUserChatAsync(long id, long idChat, string chatPosition)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                UserChat user = new UserChat(0, idChat, chatPosition);
                db.UserChats.Add(user);
                await db.SaveChangesAsync();
            }
        }
    }
}
