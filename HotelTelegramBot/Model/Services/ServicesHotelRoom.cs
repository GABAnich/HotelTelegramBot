using System.Linq;

namespace HotelTelegramBot.Model.Services
{
    class ServicesHotelRoom
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
    }
}
