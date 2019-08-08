namespace HotelTelegramBot.Logger
{
    class LogSchema
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string ChatPosition { get; set; }
        public long ChatId { get; set; }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MessageText { get; set; }
    }
}
