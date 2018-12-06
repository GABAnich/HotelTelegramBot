using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Model
{
    class Services
    {
        // public static HotelTelegramBotContext db = new HotelTelegramBotContext();

        public static async Task AddUserChatAsync(long id)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                    .Where(u => u.IdChat == id)
                    .ToArray();

                if (userChat.Length > 0)
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

                userChat.First().ChatPosition = position;
                await db.SaveChangesAsync();
            }
        }

        public static string GetChatPosition(long chatId, MessageEventArgs e)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var userChat = db.UserChats
                .Where(u => u.IdChat == e.Message.Chat.Id)
                .ToArray();

                return userChat[0].ChatPosition;
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

        public static async Task SaveUserTempDataAsync(string property, string value, long userChatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var obj = new TempInformation()
                {
                    IdUserChat = userChatId,
                    Property = property,
                    Value = value
                };
                db.TempInformation.Add(obj);

                await db.SaveChangesAsync();
            }
        }

        public static string GetUserTempData(long userChatId, string property)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var value = db.TempInformation
                         .Where(t => t.IdUserChat == userChatId)
                         .Where(t => t.Property == property)
                         .FirstOrDefault();

                return value.Value;
            }
        }

        public static async Task ClearUserTempDataAsync(long userChatId)
        {
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var value = db.TempInformation
                    .Where(t => t.IdUserChat == userChatId);

                db.TempInformation
                    .RemoveRange(value);

                await db.SaveChangesAsync();
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

        public static List<HotelRoomType> GetAviableRoomTypes(Chat userChat)
        {
            string dateOfArrival = GetUserTempData(userChat.Id, "DateOfArrival");
            string dateOfDeparture = GetUserTempData(userChat.Id, "DateOfDeparture");
            int numberOfAdults = int.Parse(GetUserTempData(userChat.Id, "NumberOfAdults"));
            int numberOfChildren = int.Parse(GetUserTempData(userChat.Id, "NumberOfChildren"));

            List<string> dates = GetIntermediateDates(dateOfArrival, dateOfDeparture);

            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                var idReservedRooms = db.HotelRoomReservedDate
                    .Where(d => dates.Contains(d.ReservedDate))
                    .Select(d => d.HotelRoomId)
                    .ToList();
                var hotelRooms = db.HotelRooms
                    .Where(r => !idReservedRooms.Contains(r.Id))
                    .Select(r => r.HotelRoomTypeId)
                    .ToList();

                return db.HotelRoomTypes
                    .Where(t => t.MaxNumberOfAdults >= numberOfAdults)
                    .Where(t => t.MaxNumberOfChildren >= numberOfChildren)
                    .Where(t => hotelRooms.Contains(t.Id))
                    .ToList();

                //return db.HotelRoomTypes
                //    .FirstOrDefault(h => h.Id == 8);
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
    }
}
