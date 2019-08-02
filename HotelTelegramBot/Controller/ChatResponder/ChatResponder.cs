using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace HotelTelegramBot.Controller
{
    class ChatResponder
    {
        private ChatState state;

        public ChatResponder(ChatState state)
        {
            SetState(state);
        }

        public void SetState(ChatState state)
        {
            this.state = state;
            this.state.SetResponder(this);
        }

        public void ReceiveMessageAsync(EventArgs e)
        {
            if ((e as MessageEventArgs).Message.Text == "Back")
            {
                state.Back();
                return;
            }

            state.ReceiveMessageAsync(e);
        }
    }
}
