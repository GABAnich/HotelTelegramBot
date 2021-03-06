﻿using HotelTelegramBot.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelTelegramBot.Model
{
    internal class DbServices
    {
        public static List<string> GetIntermediateDates(DateTime firstDate, DateTime lastDate)
        {
            return GetIntermediateDates(firstDate.ToShortDateString(), lastDate.ToShortDateString());
        }

        public static List<string> GetIntermediateDates(string firstDate, string lastDate)
        {
            List<string> list = new List<string>();

            DateTime.TryParse(firstDate, out DateTime date1);
            DateTime.TryParse(lastDate, out DateTime date2);

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
            List<string> dates = GetIntermediateDates(_r.DateOfArrival, _r.DateOfDeparture);
            long hotelRoomId = GetHotelRoom(hotelRoomTypeId, dates).Id;

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

        public static List<HotelRoomType> GetAviableRoomTypes(string dateOfArrival, string dateOfDeparture, int numberOfAdults, int numberOfChildren)
        {
            List<string> dates = GetIntermediateDates(dateOfArrival, dateOfDeparture);
            List<HotelRoom> hotelRooms = GetAviableRooms(dates);
            List<long> hotelRoomIds = GetHotelRoomTypeIds(hotelRooms);

            return ServicesHotelRoomType.GetHotelRoomTypes(numberOfAdults, numberOfChildren)
                .Where(t => hotelRoomIds.Contains(t.Id))
                .ToList();
        }

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
