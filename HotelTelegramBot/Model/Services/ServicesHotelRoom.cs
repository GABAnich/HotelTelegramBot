using System.Collections.Generic;
using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    internal class ServicesHotelRoom
    {
        public static HotelRoom GetHotelRoomById(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRooms
                    .Where(r => r.Id == id)
                    .FirstOrDefault();
            }
        }

        public static List<HotelRoom> GetHotelRoomsByTypeId(long hotelRoomTypeId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRooms
                    .Where(r => r.HotelRoomTypeId == hotelRoomTypeId)
                    .ToList();
            }
        }
    }
}
