using System;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    abstract class ChatState
    {
        protected ChatResponder responder;
        protected Chat chat;

        public ChatState(Chat chat)
        {
            this.chat = chat;
            OnCreateAsync();
        }

        public void SetResponder(ChatResponder responder)
        {
            this.responder = responder;
        }

        public abstract void ReceiveMessageAsync(EventArgs e);
        protected abstract void OnCreateAsync();
        public abstract void Back();
    }
}
