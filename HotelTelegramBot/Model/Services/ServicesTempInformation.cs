using System.Linq;
using System.Threading.Tasks;

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

        public static TempInformation GetTempInformationByChatIdAndProperty(long chatId, string property)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.TempInformation
                    .Where(t => t.IdUserChat == chatId)
                    .Where(t => t.Property == property)
                    .First();
            }
        }

        public static async Task RemoveRangeTempInformationByChatIdAsync(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var values = db.TempInformation
                    .Where(t => t.IdUserChat == chatId);

                db.TempInformation
                    .RemoveRange(values);

                await db.SaveChangesAsync();
            }
        }
    }
}
