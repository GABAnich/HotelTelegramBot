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
            string text = string.Empty;

            if ((e as MessageEventArgs) != null)
            {
                text = (e as MessageEventArgs).Message.Text;
            }
            else if ((e as CallbackQueryEventArgs) != null)
            {
                text = (e as CallbackQueryEventArgs).CallbackQuery.Data;
            }

            if (text == "Back")
            {
                state.Back();
                return;
            }

            state.ReceiveMessageAsync(e);
        }
    }
}
