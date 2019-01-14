namespace HotelTelegramBot.Model
{
    class HotelRoomReservedDate
    {
        public long Id { get; set; }
        public long ReservationId { get; set; }
        public Reservation Reservation { get; set; }

        public string ReservedDate { get; set; }
    }
}
