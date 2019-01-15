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

        internal static async Task DeleteHotelRoomReservedDateDatesAsync(List<HotelRoomReservedDate> dates)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                foreach (HotelRoomReservedDate date in dates)
                {
                    db.HotelRoomReservedDate.Attach(date);
                    db.HotelRoomReservedDate.Remove(date);
                }

                await db.SaveChangesAsync();
            }
        }

        public static List<long> GetReservationIds(List<string> dates)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomReservedDate
                    .Where(d => dates.Contains(d.ReservedDate))
                    .Select(d => d.ReservationId)
                    .ToList();
            }
        }
    }
}
