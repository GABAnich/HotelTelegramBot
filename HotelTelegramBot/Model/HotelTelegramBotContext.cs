using HotelTelegramBot.Model;
using System.Data.Entity;

namespace HotelTelegramBot
{
    class HotelTelegramBotContext : DbContext
    {
        public HotelTelegramBotContext()
            : base("HotelTelegramBotConnection")
        { }

        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<TempInformation> TempInformation { get; set; }
        public DbSet<HotelRoomType> HotelRoomTypes { get; set; }
        public DbSet<HotelRoomTypeImage> HotelRoomTypeImages { get; set; }
        public DbSet<HotelRoom> HotelRooms { get; set; }
        public DbSet<HotelRoomReservedDate> HotelRoomReservedDate { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}