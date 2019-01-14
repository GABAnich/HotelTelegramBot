using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTelegramBot.Model.Services
{
    class ServicesHotelRoomReservedDate
    {
        public static List<HotelRoomReservedDate> GetHotelRoomReservedDatesByReservationId(long reservationId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomReservedDate
                    .Where(date => date.ReservationId == reservationId)
                    .ToList();
            }
        }

        public static async Task AddHotelRoomReservedDatesAsync(long reservationId, List<string> dates)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                foreach (string date in dates)
                {
                    var reservedDate = new HotelRoomReservedDate
                    {
                        ReservationId = reservationId,
                        ReservedDate = date
                    };

                    db.HotelRoomReservedDate.Add(reservedDate); 
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
