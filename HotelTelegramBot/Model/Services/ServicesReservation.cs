using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
