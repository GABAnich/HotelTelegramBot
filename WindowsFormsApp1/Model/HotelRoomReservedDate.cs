namespace WindowsFormsApp1.Model
{
    class HotelRoomReservedDate
    {
        public long Id { get; set; }
        public long HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }

        public string ReservedDate { get; set; }
    }
}
