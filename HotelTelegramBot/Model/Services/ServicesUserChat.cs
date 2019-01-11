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
    }
}
