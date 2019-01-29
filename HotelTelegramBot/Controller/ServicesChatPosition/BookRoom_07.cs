using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_07(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            string userInput = e.Message.Text;
            await BookRoom_07(chat, userInput);
        }

        private static async Task BookRoom_07(Chat chat, string userInput)
        {
            if (!Validator.CheckName(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadName);
                return;
            }
            await DbServices.SaveUserTempDataAsync("FirstName", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть по батькові");
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 8");
        }
    }
}
