using System.Collections.Generic;

namespace HotelTelegramBot.Model
{
    class HotelRoom
    {
        public long Id { get; set; }
        public long HotelRoomTypeId { get; set; }
        public HotelRoomType HotelRoomType { get; set; }
        public ICollection<HotelRoomReservedDate> ReservationReservedDate { get; set; }
        public ICollection<Reservation> Reservation { get; set; }

        public string Name { get; set; }
        public int Floor { get; set; }
    }
}
