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
        public static async Task CrateIfNotExistUserChatAsync(long idChat)
        {
            UserChat userChat = ServicesUserChat.GetUserChatByIdChat(idChat);
            if (userChat != null) return;

            await ServicesUserChat.AddUserChatAsync(0, idChat, "/start");
        }

        public static string GetChatPositionByIdChat(long idChat)
        {
            return ServicesUserChat.GetUserChatByIdChat(idChat).ChatPosition;
        }

        public static async Task ChangePositionAsync(long idChat, string position)
        {
            await ServicesUserChat.UpdateChatPositionAsync(idChat, position);
        }

        public static async Task SaveUserTempDataAsync(string property, string value, long chatId)
        {
            await ServicesTempInformation.AddTempInformationAsync(chatId, property, value);
        }

        public static string GetUserTempDataValue(long chatId, string property)
        {
            return ServicesTempInformation.GetTempInformation(chatId, property).Value;
        }

        public static async Task ClearUserTempDataAsync(long chatId)
        {
            await ServicesTempInformation.RemoveRangeTempInformationByChatIdAsync(chatId);
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

        public static List<HotelRoom> GetAviableRooms(List<string> dates)
        {
            List<long> reservationIds = ServicesHotelRoomReservedDate.GetReservationIds(dates);
            List<long> reservedHotelRoomId = ServicesReservation.GetReservedHotelRoomIds(reservationIds);

            return ServicesHotelRoom.GetAviableHotelRooms(reservedHotelRoomId);
        }

        public static List<long> GetHotelRoomTypeIds(List<HotelRoom> rooms)
        {
            return rooms
                .Select(r => r.HotelRoomTypeId)
                .ToList();
        }

        public static List<string> GetHotelRoomTypeImagesUrl(long hotelRoomTypeId)
        {
            return ServicesHotelRoomTypeImages.GetHotelRoomTypeImages(hotelRoomTypeId)
                .Select(i => i.ImageURL)
                .ToList();
        }

        internal static async Task DeleteHotelRoomReservedDateByRoomIdAsync(long reservationId)
        {
            List<HotelRoomReservedDate> dates = ServicesHotelRoomReservedDate.GetHotelRoomReservedDatesByReservationId(reservationId);
            await ServicesHotelRoomReservedDate.DeleteHotelRoomReservedDateDatesAsync(dates);
        }

        internal static async Task<Reservation> AddReservationAsync(long chatId, long hotelRoomTypeId, Reservation _r)
        {
            //long hotelRoomTypeId = long.Parse(GetUserTempDataValue(chatId, "HotelRoomTypeId"));
            //string arrival = GetUserTempDataValue(chatId, "DateOfArrival");
            //string departure = GetUserTempDataValue(chatId, "DateOfDeparture");
            List<string> dates = GetIntermediateDates(_r.DateOfArrival, _r.DateOfDeparture);
            long hotelRoomId = GetHotelRoom(hotelRoomTypeId, dates).Id;

            //await SaveUserTempDataAsync("HotelRoomId", hotelRoomId.ToString(), chatId);
            //Reservation reservation = GetReservationFromTempData(chatId, hotelRoomId);
            _r.HotelRoomId = hotelRoomId;
            Reservation r = await ServicesReservation.AddReservationAsync(_r);
            await ServicesHotelRoomReservedDate.AddHotelRoomReservedDatesAsync(r.Id, dates);
            return r;
        }

        private static HotelRoom GetHotelRoom(long hotelRoomTypeId, List<string> dates)
        {
            return GetAviableRooms(dates)
                .Where(r => r.HotelRoomTypeId == hotelRoomTypeId)
                .FirstOrDefault();
        }

        public static List<HotelRoomType> GetAviableRoomTypes(Chat userChat, string dateOfArrival, string dateOfDeparture, int numberOfAdults, int numberOfChildren)
        {
            //string dateOfArrival = GetUserTempDataValue(userChat.Id, "DateOfArrival");
            //string dateOfDeparture = GetUserTempDataValue(userChat.Id, "DateOfDeparture");
            //int numberOfAdults = int.Parse(GetUserTempDataValue(userChat.Id, "NumberOfAdults"));
            //int numberOfChildren = int.Parse(GetUserTempDataValue(userChat.Id, "NumberOfChildren"));
            List<string> dates = GetIntermediateDates(dateOfArrival, dateOfDeparture);
            var hotelRooms = GetAviableRooms(dates);
            var hotelRoomIds = GetHotelRoomTypeIds(hotelRooms);

            return ServicesHotelRoomType.GetHotelRoomTypes(numberOfAdults, numberOfChildren)
                .Where(t => hotelRoomIds.Contains(t.Id))
                .ToList();
        }

        //private static Reservation GetReservationFromTempData(long chatId, long hotelRoomId)
        //{
        //    return new Reservation()
        //    {
        //        IdUserChat = chatId,
        //        HotelRoomId = hotelRoomId,
        //        SecondName = GetUserTempDataValue(chatId, "SecondName"),
        //        FirstName = GetUserTempDataValue(chatId, "FirstName"),
        //        MiddleName = GetUserTempDataValue(chatId, "MiddleName"),
        //        Number = GetUserTempDataValue(chatId, "Number"),
        //        Email = GetUserTempDataValue(chatId, "Email"),
        //        DateOfArrival = GetUserTempDataValue(chatId, "DateOfArrival"),
        //        DateOfDeparture = GetUserTempDataValue(chatId, "DateOfDeparture"),
        //        NumberOfAdults = int.Parse(GetUserTempDataValue(chatId, "NumberOfAdults")),
        //        NumberOfChildren = int.Parse(GetUserTempDataValue(chatId, "NumberOfChildren")),
        //    };
        //}

        private static bool IsAviableDate(string departure, DateTime lastDate)
        {
            DateTime departureDate = DateTime.Parse(departure);

            return departureDate < lastDate;
        }

        public static List<Reservation> GetValidReservation(long chatId, DateTime lastDate)
        {
            List<Reservation> reservation = ServicesReservation.GetReservationByChatId(chatId);

            // Returns actual bookings
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
}
