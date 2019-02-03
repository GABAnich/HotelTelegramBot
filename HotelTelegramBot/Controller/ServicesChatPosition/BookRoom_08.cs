using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_08(MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            Chat chat = e.Message.Chat;
            await BookRoom_08(chat, userInput);
        }

        private static async Task BookRoom_08(Chat chat, string userInput)
        {
            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("MiddleName", userInput, chat.Id);
            IReplyMarkup markup = View.Keyboards.GetRequestcontactKeyboard("📞 Мій номер");
            await ServicesMessageController.SendMessageAsync(chat, "Введіть номер телефону", markup);
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 9");
        }
    }
}
