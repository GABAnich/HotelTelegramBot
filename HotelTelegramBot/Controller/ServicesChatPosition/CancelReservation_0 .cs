using HotelTelegramBot.Model;
using HotelTelegramBot.View;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task CancelReservation_0(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;

            var listReservation = DbServices.GetValidReservation(chat.Id, DateTime.Now);
            if (listReservation.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Бронювань немає", Keyboards.ReturnMainMenu);
                await DbServices.ChangePositionAsync(chat.Id, "/start");
                return;
            }
            IReplyMarkup markup = Keyboards.GetReservationsMenu(listReservation);

            await ServicesMessageController.SendMessageAsync(chat, "Бронювання: ", markup);
            await DbServices.ChangePositionAsync(chat.Id, "❌ Зняти бронювання 1");
        }
    }
}
