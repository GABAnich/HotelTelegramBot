using System;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class ChatResponder
    {
        private ChatState state;
        private Chat chat;
        public Dictionary<string, string> userTempData = new Dictionary<string, string>();

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
            Logger.Logger.Log(state.GetType().ToString(), e);

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
