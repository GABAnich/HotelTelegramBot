namespace HotelTelegramBot.Model
{
    class HotelRoomTypeImage
    {
        public long Id { get; set; }
        public long? HotelRoomTypeId { get; set; }
        public HotelRoomType HotelRoomType { get; set; }

        public string ImageURL { get; set; }
    }
}
