using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    class ServicesUserChat
    {
        public static UserChat GetUserChatByChatId(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.UserChats
                    .Where(u => u.IdChat == chatId)
                    .FirstOrDefault();
            }
        }

        public static string GetChatPositionById(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == chatId)
                    .FirstOrDefault();

                return userChat.ChatPosition;
            }
        }

        public static async void AddUserChatAsync(long id, long idChat, string chatPosition)
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
