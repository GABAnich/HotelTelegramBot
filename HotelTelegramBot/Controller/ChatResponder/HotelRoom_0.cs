using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    internal class HotelRoom_0 : ChatState
    {
        public HotelRoom_0(Chat chat) : base(chat)
        {
        }

        public override void Back()
        {
            responder.SetState(new Start(chat));
        }

        public override async Task ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as CallbackQueryEventArgs).CallbackQuery.Data;

            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }

            long roomTypeId = int.Parse(userInput);
            HotelRoomType roomType = ServicesHotelRoomType.GetHotelRoomTypeById(roomTypeId);
            List<string> photos = DbServices.GetHotelRoomTypeImagesUrl(roomTypeId);

            if (roomType == null)
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "Такого типу номеру не існує", Keyboards.ReturnMainMenu);
                return;
            }

            string message = ViewRoomType.GetTextAboutRoomType(roomType);
            await ServicesMessageController.SendPhotosAsync(chat.Id, photos);
            await ServicesMessageController.SendMessageAsync(chat, message, Keyboards.ReturnMainMenu);
        }

        protected override async void OnCreateAsync()
        {
            List<HotelRoomType> listRoomTypes = ServicesHotelRoomType.GetHotelRoomTypes();

            if (listRoomTypes.Count == 0)
            {
                await ServicesMessageController.SendMessageAsync(chat, "Номерів немає", Keyboards.ReturnMainMenu);
                return;
            }

            IReplyMarkup markup = Keyboards.GetRoomTypesMenu(listRoomTypes);
            await ServicesMessageController.SendMessageAsync(chat, "Оберіть тип номеру", markup);
        }
    }
}