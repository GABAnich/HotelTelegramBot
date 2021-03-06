﻿using System;
using System.Text.RegularExpressions;

namespace HotelTelegramBot.Controller
{
    public class Validator
    {
        public static readonly string BadDateFormat = "Введіть дату у форматі day/month/yaer. Приклад: 01/12/2001";
        public static readonly string BadDateLessCurrent = "Введіть дату, яка більша за поточну";
        public static readonly string BadDateRange = "Наступна дата повина бути більша за попередню";
        public static readonly string BadNumber = "Введіть число";
        public static readonly string BadNumberRange = "Введіть число в діапазоні 0-10";
        public static readonly string BadEmail = "Введіть коректний e-mail";
        public static readonly string BadPhoneNumber = "Введіть коректний номер телефону. Приклад: +380501001214";
        public static readonly string BadName = "Можна використовувати лише символи A-я A-z";

        public static bool CheckDateFormat(string userDate)
        {
            DateTime date = new DateTime();

            return DateTime.TryParse(userDate, out date);
        }

        public static bool CheckDateBigerCurrent(string userDate)
        {
            DateTime date = new DateTime();
            DateTime.TryParse(userDate, out date);

            return date > DateTime.Today;
        }

        public static bool CheckDateRange(string start, string end)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            DateTime.TryParse(start, out startDate);
            DateTime.TryParse(end, out endDate);

            return endDate > startDate;
        }

        public static bool CheckNumber(string userNumber)
        {
            return int.TryParse(userNumber, out int number);
        }

        public static bool CheckNumberRange(string userNumber)
        {
            int.TryParse(userNumber, out int number);

            return number >= 0 && number <= 10;
        }

        public static bool CheckEmail(string userEmail)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(userEmail);
                return addr.Address == userEmail;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{12})$").Success;
        }

        public static bool CheckName(string name)
        {
            return Regex.IsMatch(name, @"^[a-zA-ZА-Яа-яІіЇїЄєЬь]+$");
        }
    }
}
