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
    }
}
