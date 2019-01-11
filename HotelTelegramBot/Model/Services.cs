using HotelTelegramBot.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Model
{
    internal class DbServices
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

        public static List<HotelRoomType> GetRoomTypes()
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .ToList();
            }
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

        public static List<long> GetHotelRoomTypeIds(List<HotelRoom> rooms)
        {
            return rooms
                .Select(r => r.HotelRoomTypeId)
                .ToList();
        }

        public static List<string> GetHotelRoomTypeImagesUrl(long hotelRoomTypeId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypeImages
                    .Where(i => i.HotelRoomTypeId == hotelRoomTypeId)
                    .Select(i => i.ImageURL)
                    .ToList();
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
            var hotelRoomIds = GetHotelRoomTypeIds(hotelRooms);

            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomTypes
                    .Where(t => t.MaxNumberOfAdults >= numberOfAdults)
                    .Where(t => t.MaxNumberOfChildren >= numberOfChildren)
                    .Where(t => hotelRoomIds.Contains(t.Id))
                    .ToList();
            }
        }

        // Should delete hotelRoomReservedDates from reservation
        // But deleting all reservedDates by RoomId
        internal static async Task DeleteHotelRoomReservedDateByRoomIdAsync(long hotelRoomId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                List<HotelRoomReservedDate> dates = GetHotelRoomReservedDateByIdHotelRoom(hotelRoomId);

                foreach (HotelRoomReservedDate date in dates)
                {
                    db.HotelRoomReservedDate.Attach(date);
                    db.HotelRoomReservedDate.Remove(date);
                }

                await db.SaveChangesAsync();
            }
        }

        private static List<HotelRoomReservedDate> GetHotelRoomReservedDateByIdHotelRoom(long hotelRoomId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                return db.HotelRoomReservedDate
                    .Where(date => date.HotelRoomId == hotelRoomId)
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

        internal static async Task<Reservation> AddReservationAsync(long chatId)
        {
            UserChat userChat = GetUserChatByChatId(chatId);
            long hotelRoomId;
            long hotelRoomTypeId = long.Parse(GetUserTempData(chatId, "HotelRoomTypeId"));
            List<string> dates = GetIntermediateDates(
                        GetUserTempData(chatId, "DateOfArrival"),
                        GetUserTempData(chatId, "DateOfDeparture"));

            // Geting first aviable room
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                hotelRoomId = GetAviableRooms(dates)
                    .Where(r => r.HotelRoomTypeId == hotelRoomTypeId)
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

            Reservation res = await ServicesReservation.AddReservation(reservation);
            return res;
        }

        private static bool IsAviableDate(string departure, DateTime lastDate)
        {
            DateTime departureDate = DateTime.Parse(departure);

            return departureDate < lastDate;
        }

        public static List<Reservation> GetValidReservation(long chatId, DateTime lastDate)
        {
            UserChat userChat = GetUserChatByChatId(chatId);
            List<Reservation> reservation = ServicesReservation.GetReservationByChatId(userChat.Id);

            // Returns current bookings
            for (int i = reservation.Count - 1; i > -1; i--)
            {
                if (IsAviableDate(reservation[i].DateOfDeparture, lastDate))
                {
                    reservation.RemoveAt(i);
                }
            }
            
            return reservation;
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
