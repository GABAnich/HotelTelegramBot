using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task BookRoom_09(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            string userInput;

            if (e.Message.Type == MessageType.Text)
            {
                userInput = e.Message.Text;
            }
            else if (e.Message.Type == MessageType.Contact)
            {
                userInput = e.Message.Contact.PhoneNumber;
            }
            else
            {
                return;
            }
            await BookRoom_09(chat, userInput);
        }

        private static async Task BookRoom_09(Chat chat, string userInput)
        {
            if (!Validator.CheckPhoneNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadPhoneNumber);
                return;
            }
            await DbServices.SaveUserTempDataAsync("Number", userInput, chat.Id);
            await ServicesMessageController.SendMessageAsync(chat, "Введіть Email");
            await DbServices.ChangePositionAsync(chat.Id, "🏨 Замовити номер 10");
        }
    }
}
