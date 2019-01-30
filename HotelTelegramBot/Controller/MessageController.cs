using HotelTelegramBot.Model;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HotelTelegramBot.Controller
{
    class MessageController
    {
        internal static async void OnMessageAsync(object sender, MessageEventArgs e)
        {
            string userInput = e.Message.Text;
            Chat chat = e.Message.Chat;
            string chatPosition;

            if (e.Message == null)
            {
                return;
            }

            try
            {
                await DbServices.CrateIfNotExistUserChatAsync(chat.Id);
                await ServicesMessageController.RouteMenuAsync(userInput, chat);

                chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
                Logger.Log(chatPosition, e);
                await ServicesMessageController.RouteMessageChatPositionAsync(chatPosition, e);
            }
            catch(Telegram.Bot.Exceptions.ApiRequestException exception)
            {
                if (exception.Message == "Forbidden: bot was blocked by the user")
                {
                    Logger.Log("Forbidden: bot was blocked by the user");
                    return;
                }
            }
        }

        internal static async void OnCallbackQueryAsync(object sender, CallbackQueryEventArgs e)
        {
            Chat chat = e.CallbackQuery.Message.Chat;
            int messageId = e.CallbackQuery.Message.MessageId;
            string chatPosition = DbServices.GetChatPositionByIdChat(chat.Id);
            string userInput = e.CallbackQuery.Data;

            Logger.Log(chatPosition, e);

            await Program.botClient.DeleteMessageAsync(chat, messageId);
            await ServicesMessageController.RouteMenuAsync(userInput, chat);
            await RouteMessageChatPositionAsync(chatPosition, e);
        }

        private static async Task RouteMessageChatPositionAsync(string chatPosition, CallbackQueryEventArgs e)
        {
            if (chatPosition == "⛺️ Номери 1")
            {
                await ServicesChatPosition.HotelRoom_1(e);
            }
            else if (chatPosition == "❌ Зняти бронювання 1")
            {
                await ServicesChatPosition.CancelReservation_1(e);
            }
            else if (chatPosition == "🏨 Замовити номер 5")
            {
                await ServicesChatPosition.BookRoom_05(e);
            }
        }
    }
}
