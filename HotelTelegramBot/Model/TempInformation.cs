namespace HotelTelegramBot.Model
{
    class TempInformation
    {
        public long Id { get; set; }
        public long? IdUserChat { get; set; }
        public UserChat UserChat { get; set; }

        public string Property { get; set; }
        public string Value { get; set; }
    }
}
