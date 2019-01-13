using System.Collections.Generic;
using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    class ServicesHotelRoomTypeImages
    {
        public static List<HotelRoomTypeImage> GetHotelRoomTypeImages(long hotelRoomTypeId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypeImages
                    .Where(i => i.HotelRoomTypeId == hotelRoomTypeId)
                    .ToList();
            }
        }
    }
}
