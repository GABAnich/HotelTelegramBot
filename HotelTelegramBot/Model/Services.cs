using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Model
{
    class Services
    {
        public static async Task AddUserChatAsync(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == id)
                    .FirstOrDefault();

                if (userChat != null)
                {
                    return;
                }

                try
                {
                    UserChat user = new UserChat(0, id, "/start");
                    db.UserChats.Add(user);
                    await db.SaveChangesAsync();
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        public static async Task ChangePositionAsync(long id, string position)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == id);

                userChat
                    .FirstOrDefault().ChatPosition = position;
                await db.SaveChangesAsync();
            }
        }

        public static string GetChatPosition(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == chatId)
                    .FirstOrDefault();

                return userChat.ChatPosition;
            }
        }

        public static string GetInfoAboutHotel()
        {
            return AboutHotel.InfoAboutHotel;
        }

        public static string GetImageAboutHotel()
        {
            return AboutHotel.ImageAboutHotel;
        }

        public static async Task SaveUserTempDataAsync(string property, string value, long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var obj = new TempInformation()
                {
                    IdUserChat = chatId,
                    Property = property,
                    Value = value
                };
                db.TempInformation.Add(obj);

                await db.SaveChangesAsync();
            }
        }

        public static string GetUserTempData(long chatId, string property)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var value = db.TempInformation
                         .Where(t => t.IdUserChat == chatId)
                         .Where(t => t.Property == property)
                         .First();

                return value.Value;
            }
        }

        public static async Task ClearUserTempDataAsync(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var value = db.TempInformation
                    .Where(t => t.IdUserChat == chatId);

                db.TempInformation
                    .RemoveRange(value);

                await db.SaveChangesAsync();
            }
        }

        public static List<string> GetIntermediateDates(DateTime firstDate, DateTime lastDate)
        {
            return GetIntermediateDates(firstDate.ToShortDateString(), lastDate.ToShortDateString());
        }

        public static List<string> GetIntermediateDates(string firstDate, string lastDate)
        {
            List<string> list = new List<string>();

            DateTime date1 = new DateTime();
            DateTime.TryParse(firstDate, out date1);

            DateTime date2 = new DateTime();
            DateTime.TryParse(lastDate, out date2);

            list.Add(date1.ToShortDateString());
            while (date1 != date2)
            {
                date1 = date1.AddDays(1);
                list.Add(date1.ToShortDateString());
            }

            return list;
        }

        public static UserChat GetUserChatByChatId(long chatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.UserChats
                    .Where(u => u.IdChat == chatId)
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

        public static List<HotelRoom> GetAviableRooms(List<string> dates)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var idReservedRooms = db.HotelRoomReservedDate
                    .Where(d => dates.Contains(d.ReservedDate))
                    .Select(d => d.HotelRoomId)
                    .ToList();

                return db.HotelRooms
                    .Where(r => !idReservedRooms.Contains(r.Id))
                    .ToList();
            }
        }

        public static HotelRoom GetHotelRoomById(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRooms
                    .Where(r => r.Id == id)
                    .FirstOrDefault();
            }
        }

        public static List<long> GetRoomTypeIds(List<HotelRoom> rooms)
        {
            return rooms
                .Select(r => r.HotelRoomTypeId)
                .ToList();
        }

        public static HotelRoomType GetHotelRoomTypeById(long hotelRoomTypeId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.Id == hotelRoomTypeId)
                    .FirstOrDefault();
            }
        }

        public static HotelRoomType GetHotelRoomTypeByName(string hotelRoomTypeName)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.Name == hotelRoomTypeName)
                    .FirstOrDefault();
            }
        }

        public static List<HotelRoomType> GetAviableRoomTypes(Chat userChat)
        {
            string dateOfArrival = GetUserTempData(userChat.Id, "DateOfArrival");
            string dateOfDeparture = GetUserTempData(userChat.Id, "DateOfDeparture");
            int numberOfAdults = int.Parse(GetUserTempData(userChat.Id, "NumberOfAdults"));
            int numberOfChildren = int.Parse(GetUserTempData(userChat.Id, "NumberOfChildren"));

            List<string> dates = GetIntermediateDates(dateOfArrival, dateOfDeparture);

            var hotelRooms = GetAviableRooms(dates);
            var hotelRoomIds = GetRoomTypeIds(hotelRooms);

            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.MaxNumberOfAdults >= numberOfAdults)
                    .Where(t => t.MaxNumberOfChildren >= numberOfChildren)
                    .Where(t => hotelRoomIds.Contains(t.Id))
                    .ToList();
            }
        }

        public static List<HotelRoomTypeImage> GetRoomTypeImages(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypeImages
                    .Where(i => i.HotelRoomTypeId == id)
                    .ToList();
            }
        }

        internal static async Task AddReservationAsync(long chatId)
        {
            UserChat userChat = GetUserChatByChatId(chatId);
            long hotelRoomId;
            string hotelRoomTypeName = GetUserTempData(chatId, "HotelRoomTypeName");
            HotelRoomType hotelRoomType = GetHotelRoomTypeByName(hotelRoomTypeName);
            List<string> dates = GetIntermediateDates(
                        GetUserTempData(chatId, "DateOfArrival"),
                        GetUserTempData(chatId, "DateOfDeparture"));

            // Geting first aviable room
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                hotelRoomId = GetAviableRooms(dates)
                    .Where(r => r.HotelRoomTypeId == hotelRoomType.Id)
                    .FirstOrDefault().Id;
            }

            AddReservedDatesAsync(hotelRoomId, dates);

            var reservation = new Reservation()
            {
                IdUserChat = userChat.Id,
                HotelRoomId = hotelRoomId,
                SecondName = GetUserTempData(chatId, "SecondName"),
                FirstName = GetUserTempData(chatId, "FirstName"),
                MiddleName = GetUserTempData(chatId, "MiddleName"),
                Number = GetUserTempData(chatId, "Number"),
                Email = GetUserTempData(chatId, "Email"),
                DateOfArrival = GetUserTempData(chatId, "DateOfArrival"),
                DateOfDeparture = GetUserTempData(chatId, "DateOfDeparture"),
                NumberOfAdults = int.Parse(GetUserTempData(chatId, "NumberOfAdults")),
                NumberOfChildren = int.Parse(GetUserTempData(chatId, "NumberOfChildren")),
            };

            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                db.Reservations.Add(reservation);
                await db.SaveChangesAsync();
            }
        }

        private static bool IsAviableDate(string departure, DateTime lastDate)
        {
            DateTime departureDate = DateTime.Parse(departure);

            return departureDate < lastDate;
        }

        public static List<Reservation> GetReservation(long chatId, DateTime lastDate)
        {
            UserChat userChat = GetUserChatByChatId(chatId);

            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                List<Reservation> reservation = db.Reservations
                    .Where(r => r.IdUserChat == userChat.Id)
                    .ToList();

                for (int i = reservation.Count - 1; i > -1; i--)
                {
                    if (IsAviableDate(reservation[i].DateOfDeparture, lastDate))
                    {
                        reservation.RemoveAt(i);
                    }
                }

                return reservation;
            }
        }

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

        private static async void AddReservedDatesAsync(long hotelRoomId, List<string> dates)
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
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
