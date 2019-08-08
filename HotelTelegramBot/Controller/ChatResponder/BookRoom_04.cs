using HotelTelegramBot.Model;
using HotelTelegramBot.Model.Services;
using HotelTelegramBot.View;
using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.Controller
{
    internal class BookRoom_04 : ChatState
    {
        private string arrival;
        private string departure;
        private string adults;
        private string children;

        public override async void OnStateChange(Chat chat)
        {
            responder.userTempData.TryGetValue("DateOfArrival", out arrival);
            responder.userTempData.TryGetValue("DateOfDeparture", out departure);
            responder.userTempData.TryGetValue("NumberOfAdults", out adults);
            responder.userTempData.TryGetValue("NumberOfChildren", out children);

            var listRoomTypes = DbServices.GetAviableRoomTypes(chat, arrival, departure, int.Parse(adults), int.Parse(children));

            if (listRoomTypes.Count <= 0)
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "На вказаний період немає доступних номерів.", Keyboards.ReturnMainMenu);
            }

            IReplyMarkup markup = Keyboards.GetRoomTypesMenu(listRoomTypes, "Замовити: ");
            await ServicesMessageController.SendMessageAsync(chat, "Оберіть тип номеру", markup);
        }

        public override async void ReceiveMessageAsync(EventArgs e)
        {
            string userInput = (e as CallbackQueryEventArgs).CallbackQuery.Data;
            Chat chat = (e as CallbackQueryEventArgs).CallbackQuery.Message.Chat;

            if (!Validator.CheckNumber(userInput))
            {
                await ServicesMessageController.SendMessageAsync(chat, Validator.BadNumber);
                return;
            }
            long id = long.Parse(userInput);
            var listRoomTypes = DbServices.GetAviableRoomTypes(chat, arrival, departure, int.Parse(adults), int.Parse(children));
            if (ServicesHotelRoomType.GetHotelRoomTypeById(id) == null ||
                !listRoomTypes.Exists(t => t.Id == id))
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "Оберіть тип номеру", Keyboards.ReturnMainMenu);
                return;
            };

            responder.userTempData["HotelRoomTypeId"] = userInput;
            responder.SetState(new BookRoom_05());
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_03());
        }
    }
}