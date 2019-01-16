using System.Collections.Generic;
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

        public static List<HotelRoomType> GetHotelRoomTypes(int numberOfAdults, int numberOfChildren)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.MaxNumberOfAdults >= numberOfAdults)
                    .Where(t => t.MaxNumberOfChildren >= numberOfChildren)
                    .ToList();
            }
        }

        public static List<HotelRoomType> GetRoomTypes()
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .ToList();
            }
        }
    }
}
