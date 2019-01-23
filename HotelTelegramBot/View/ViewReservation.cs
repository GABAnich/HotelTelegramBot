using HotelTelegramBot.Model;

namespace HotelTelegramBot.View
{
    class ViewReservation
    {
        public static string GetTextAboutReservation(Reservation r, HotelRoomType t, HotelRoom room, int countDays)
        {
            return "" +
                $"*{t.Name}\n*" +
                $"\n" +
                $"Прізвище: {r.SecondName}\n" +
                $"Ім’я: {r.FirstName}\n" +
                $"По батькові: {r.MiddleName}\n" +
                $"Номер телефону: {r.Number}\n" +
                $"Email: {r.Email}\n" +
                $"Період: {r.DateOfArrival}-{r.DateOfDeparture}\n" +
                $"Дорослих: {r.NumberOfAdults}\n" +
                $"Дітей: {r.NumberOfChildren}\n" +
                $"\n" +
                $"Кімната: {room.Name}\n" +
                $"Поверх: {room.Floor}\n" +
                $"\n" +
                $"До оплати: {countDays * t.Price} грн\n" +
                $"\n" +
                $"Ідентифікатор для перевірки: *494ebf5f419ad02a86af25f8db5ed114790399c2aa6b233384b1b4b9ac3458e5*";
        }
    }
}
