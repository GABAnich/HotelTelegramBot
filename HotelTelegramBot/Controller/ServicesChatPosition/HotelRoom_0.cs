using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    partial class ServicesChatPosition
    {
        internal static async Task HotelRoom_0(MessageEventArgs e)
        {
            Chat chat = e.Message.Chat;
            await HotelRoom_0(chat);
        }

        private static async Task HotelRoom_0(Chat chat)
        {
            List<HotelRoomType> listRoomTypes = ServicesHotelRoomType.GetHotelRoomTypes();

            if (listRoomTypes.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Номерів немає", Keyboards.ReturnMainMenu);
                return;
            }

            IReplyMarkup markup = Keyboards.GetRoomTypesMenu(listRoomTypes);
            await ServicesMessageController.SendMessageAsync(chat, "Оберіть тип номеру", markup);
            await DbServices.ChangePositionAsync(chat.Id, "⛺️ Номери 1");
        }
    }
}
