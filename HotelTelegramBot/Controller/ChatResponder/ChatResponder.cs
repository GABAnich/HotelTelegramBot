using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class ChatResponder
    {
        private ChatState state;
        private Chat chat;

        public ChatResponder(Chat chat, ChatState state)
        {
            this.chat = chat;
            SetState(state);
        }

        public void SetState(ChatState state)
        {
            this.state = state;
            this.state.SetResponder(this);
            this.state.OnStateChange(chat);
        }

        public void ReceiveMessageAsync(EventArgs e)
        {
            string text = string.Empty;

            if ((e as MessageEventArgs) != null)
            {
                text = (e as MessageEventArgs).Message.Text;
                // Move this code to another place
                Logger.Log(state.GetType().ToString(), e as MessageEventArgs);
            }
            else if ((e as CallbackQueryEventArgs) != null)
            {
                text = (e as CallbackQueryEventArgs).CallbackQuery.Data;
                // Move this code to another place
                Logger.Log(state.GetType().ToString(), e as CallbackQueryEventArgs);
            }

            if (text == "Back")
            {
                state.Back();
                return;
            }
            else if (text == "/start")
            {
                SetState(new Start());
            }
            else if (text == "/main_menu" || text == "🎛 Головне меню")
            {
                SetState(new MainMenu());
            }

            state.ReceiveMessageAsync(e);
        }
    }
}
