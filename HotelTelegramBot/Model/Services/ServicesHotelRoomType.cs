using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    class ServicesHotelRoomType
    {
        public static HotelRoomType GetHotelRoomTypeById(long hotelRoomTypeId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.Id == hotelRoomTypeId)
                    .FirstOrDefault();
            }
        }

        public static HotelRoomType GetHotelRoomTypeByName(string hotelRoomTypeName)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.Name == hotelRoomTypeName)
                    .FirstOrDefault();
            }
        }
    }
}
