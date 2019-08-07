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
        public override async void OnCreateAsync(Chat chat)
        {
            var listRoomTypes = DbServices.GetAviableRoomTypes(chat);

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

            if (ServicesHotelRoomType.GetHotelRoomTypeById(id) == null ||
                !DbServices.GetAviableRoomTypes(chat).Exists(t => t.Id == id))
            {
                await ServicesMessageController.SendMessageAsync(
                    chat, "Оберіть тип номеру", Keyboards.ReturnMainMenu);
                return;
            };

            await DbServices.SaveUserTempDataAsync("HotelRoomTypeId", userInput, chat.Id);

            responder.SetState(new BookRoom_05());
        }

        public override void Back()
        {
            responder.SetState(new BookRoom_03());
        }
    }
}