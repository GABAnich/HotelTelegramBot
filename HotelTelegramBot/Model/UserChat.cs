using HotelTelegramBot.Model;
using System.Collections.Generic;

namespace HotelTelegramBot
{
    class UserChat
    {
        public long Id { get; set; }
        public ICollection<TempInformation> TempInformation { get; set; }
        public ICollection<Reservation> Reservation { get; set; }

        public long IdChat { get; set; }
        public string ChatPosition { get; set; }       

        public UserChat()
        { }

        public UserChat(long id, long idChat, string chatPosition)
        {
            Id = id;
            IdChat = idChat;
            ChatPosition = chatPosition;
        }
    }
}
