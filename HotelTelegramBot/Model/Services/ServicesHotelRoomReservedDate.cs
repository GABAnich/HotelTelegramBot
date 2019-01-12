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

        public static async void AddHotelRoomReservedDatesAsync(long hotelRoomId, List<string> dates)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                foreach (string date in dates)
                {
                    var reservedDate = new HotelRoomReservedDate
                    {
                        HotelRoomId = hotelRoomId,
                        ReservedDate = date
                    };

                    db.HotelRoomReservedDate.Add(reservedDate); 
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
