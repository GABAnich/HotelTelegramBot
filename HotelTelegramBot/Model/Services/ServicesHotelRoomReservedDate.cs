using System.Collections.Generic;
using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    class ServicesHotelRoomReservedDate
    {
        public static List<HotelRoomReservedDate> GetHotelRoomReservedDatesByHotelRoomId(long hotelRoomId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomReservedDate
                    .Where(date => date.HotelRoomId == hotelRoomId)
                    .ToList();
            }
        }
    }
}
