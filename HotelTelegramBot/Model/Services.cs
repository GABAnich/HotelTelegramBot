﻿using HotelTelegramBot.Model.Services;
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
            return ServicesTempInformation.GetTempInformationByChatIdAndProperty(chatId, property).Value;
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
            string dateOfArrival = GetUserTempDataValue(userChat.Id, "DateOfArrival");
            string dateOfDeparture = GetUserTempDataValue(userChat.Id, "DateOfDeparture");
            int numberOfAdults = int.Parse(GetUserTempDataValue(userChat.Id, "NumberOfAdults"));
            int numberOfChildren = int.Parse(GetUserTempDataValue(userChat.Id, "NumberOfChildren"));

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
                List<HotelRoomReservedDate> dates = ServicesHotelRoomReservedDate.GetHotelRoomReservedDatesByHotelRoomId(hotelRoomId);

                foreach (HotelRoomReservedDate date in dates)
                {
                    db.HotelRoomReservedDate.Attach(date);
                    db.HotelRoomReservedDate.Remove(date);
                }

                await db.SaveChangesAsync();
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
            UserChat userChat = ServicesUserChat.GetUserChatByIdChat(chatId);
            long hotelRoomId;
            long hotelRoomTypeId = long.Parse(GetUserTempDataValue(chatId, "HotelRoomTypeId"));
            string arrival = GetUserTempDataValue(chatId, "DateOfArrival");
            string departure = GetUserTempDataValue(chatId, "DateOfDeparture");
            List<string> dates = GetIntermediateDates(arrival, departure);

            // Geting first aviable room
            using (HotelTelegramBotContext db = new HotelTelegramBotContext())
            {
                hotelRoomId = GetAviableRooms(dates)
                    .Where(r => r.HotelRoomTypeId == hotelRoomTypeId)
                    .FirstOrDefault().Id;
            }

            var reservation = new Reservation()
            {
                IdUserChat = userChat.Id,
                HotelRoomId = hotelRoomId,
                SecondName = GetUserTempDataValue(chatId, "SecondName"),
                FirstName = GetUserTempDataValue(chatId, "FirstName"),
                MiddleName = GetUserTempDataValue(chatId, "MiddleName"),
                Number = GetUserTempDataValue(chatId, "Number"),
                Email = GetUserTempDataValue(chatId, "Email"),
                DateOfArrival = GetUserTempDataValue(chatId, "DateOfArrival"),
                DateOfDeparture = GetUserTempDataValue(chatId, "DateOfDeparture"),
                NumberOfAdults = int.Parse(GetUserTempDataValue(chatId, "NumberOfAdults")),
                NumberOfChildren = int.Parse(GetUserTempDataValue(chatId, "NumberOfChildren")),
            };

            await ServicesHotelRoomReservedDate.AddHotelRoomReservedDatesAsync(hotelRoomId, dates);
            return await ServicesReservation.AddReservation(reservation);
        }

        private static bool IsAviableDate(string departure, DateTime lastDate)
        {
            DateTime departureDate = DateTime.Parse(departure);

            return departureDate < lastDate;
        }

        public static List<Reservation> GetValidReservation(long chatId, DateTime lastDate)
        {
            UserChat userChat = ServicesUserChat.GetUserChatByIdChat(chatId);
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
    }
}
