using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTelegramBot.Model.Services
{
    class ServicesReservation
    {
        public static Reservation GetReservationById(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.Reservations
                    .Where(r => r.Id == id)
                    .FirstOrDefault();
            }
        }

        public static List<Reservation> GetReservationByChatId(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.Reservations
                    .Where(r => r.IdUserChat == chatId)
                    .ToList();
            }
        }

        public static async Task DeleteReservationById(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                Reservation r = GetReservationById(id);

                db.Reservations.Attach(r);
                db.Reservations.Remove(r);

                await db.SaveChangesAsync();
            }
        }

        public static async Task<Reservation> AddReservation(Reservation reservation)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                Reservation r = db.Reservations.Add(reservation);
                await db.SaveChangesAsync();
                return r;
            }
        }

        public static List<long> GetReservedHotelRoomIds(List<long> reservationIds)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.Reservations
                    .Where(r => reservationIds.Contains(r.Id))
                    .Select(r => r.HotelRoomId)
                    .ToList();
            }
        }
    }
}
