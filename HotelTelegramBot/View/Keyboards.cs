using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace HotelTelegramBot.View
{
    class Keyboards
    {
        static readonly public ReplyKeyboardMarkup MainKeyboard = new ReplyKeyboardMarkup
        {
            Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[] { "🏨 Замовити номер", "❌ Зняти бронювання" },
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        static readonly public ReplyKeyboardMarkup ReturnMainMenu = new ReplyKeyboardMarkup
        {
            Keyboard = new KeyboardButton[][]
            {
                new KeyboardButton[] { "🎛 Головне меню" },
            },
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        static public ReplyKeyboardMarkup NextDates(List<string> dates)
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

        static public ReplyKeyboardMarkup Text(string text)
        {
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

        static public ReplyKeyboardMarkup Adults
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

        static public ReplyKeyboardMarkup Children
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
    }
}
