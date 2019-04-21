namespace HotelTelegramBot.Model
{
    class Reservation
    {
        public long Id { get; set; }
        public long HotelRoomId { get; set; }
        public HotelRoom HotelRoom { get; set; }
        public long IdUserChat { get; set; }
        public UserChat UserChat { get; set; }

        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Number { get; set; }
        public string Email { get; set; }
        public string DateOfArrival { get; set; }
        public string DateOfDeparture { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }

    }
}
