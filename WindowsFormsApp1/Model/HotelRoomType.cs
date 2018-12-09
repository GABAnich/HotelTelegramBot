using System.Collections.Generic;

namespace WindowsFormsApp1.Model
{
    class HotelRoomType
    {
        public long Id { get; set; }
        public ICollection<HotelRoomTypeImage> HotelRoomTypeImage { get; set; }
        public ICollection<HotelRoom> HotelRoom { get; set; }

        public string Name { get; set; }
        public int MaxNumberOfAdults { get; set; }
        public int MaxNumberOfChildren { get; set; }
        public string Services { get; set; }
        public double Area { get; set; }
        public string Description { get; set; }
        public int TotalCountRoom { get; set; }
        public double Price { get; set; }
    }
}