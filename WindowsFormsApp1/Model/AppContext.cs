using System.Data.Entity;

namespace WindowsFormsApp1.Model
{
    class AppContext : DbContext
    {
        public AppContext()
            : base("HotelTelegramBotConnection")
        { }

        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<TempInformation> TempInformation { get; set; }
        public DbSet<HotelRoomType> HotelRoomTypes { get; set; }
        public DbSet<HotelRoomTypeImage> HotelRoomTypeImages { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelRoomReservedDate> HotelRoomReservedDate { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Operator> Operators { get; set; }
    }
}
