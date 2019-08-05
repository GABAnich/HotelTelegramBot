using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

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
            Chat chat = null;

            if ((e as MessageEventArgs) != null)
            {
                text = (e as MessageEventArgs).Message.Text;
                chat = (e as MessageEventArgs).Message.Chat;
            }
            else if ((e as CallbackQueryEventArgs) != null)
            {
                text = (e as CallbackQueryEventArgs).CallbackQuery.Data;
                chat = (e as CallbackQueryEventArgs).CallbackQuery.Message.Chat;
            }

            if (text == "Back")
            {
                state.Back();
                return;
            }
            else if (text == "/start")
            {
                SetState(new Start(chat));
            }
            else if (text == "🎛 Головне меню")
            {
                SetState(new MainMenu(chat));
            }

            state.ReceiveMessageAsync(e);
        }
    }
}
