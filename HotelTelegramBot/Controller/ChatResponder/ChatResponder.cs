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
            //Change name 'OnCreateAsync' to 'OnStateChange'
            this.state.OnCreateAsync(chat);
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
