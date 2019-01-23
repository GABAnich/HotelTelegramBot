using HotelTelegramBot.Model;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.View
{
    class Keyboards
    {
        static readonly public IReplyMarkup MainKeyboard = new ReplyKeyboardMarkup
        {
            Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[] { "⛺️ Номери", "❌ Зняти бронювання" },
                new KeyboardButton[] { "🏨 Замовити номер" },
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        static readonly public IReplyMarkup ReturnMainMenu = new ReplyKeyboardMarkup
        {
            Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[] { "🎛 Головне меню" },
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        static public IReplyMarkup NextDates(List<string> dates)
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[] { dates[0],  dates[1], dates[2] },
                    new KeyboardButton[] { dates[3],  dates[4], dates[5] },
                },
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

        static public IReplyMarkup Text(string text)
        {
            if (text == null)
            {
                return new ReplyKeyboardRemove();
            }
            else if (text.Length == 0)
            {
                return new ReplyKeyboardRemove();
            }

            return new ReplyKeyboardMarkup
            {
                Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[] { text },
                },
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }

        static public IReplyMarkup Adults
        {
            get
            {
                return new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[] { "1", "2", "3", "4", "5" },
                },
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
            }
        }

        static public IReplyMarkup Children
        {
            get
            {
                return new ReplyKeyboardMarkup
                {
                    Keyboard = new KeyboardButton[][]
                {
                    new KeyboardButton[] { "0", "1", "2", "3" },
                },
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };
            }
        }

        static public IReplyMarkup GetRoomTypesMenu(List<HotelRoomType> listRoomTypes, string text = "")
        {
            List<List<InlineKeyboardButton>> keyboards = new List<List<InlineKeyboardButton>>();
            foreach (HotelRoomType t in listRoomTypes)
            {
                keyboards.Add(new List<InlineKeyboardButton>() {
                    InlineKeyboardButton.WithCallbackData($"{text} {t.Name}", $"{t.Id}")
                });
            }

            return new InlineKeyboardMarkup(keyboards);
        }
    }
}
