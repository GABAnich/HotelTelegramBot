using System;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    abstract class ChatState
    {
        protected ChatResponder responder;

        public void SetResponder(ChatResponder responder)
        {
            this.responder = responder;
        }

        public abstract void ReceiveMessageAsync(EventArgs e);
        public abstract void OnCreateAsync(Chat chat);
        public abstract void Back();
    }
}
