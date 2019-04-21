using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelTelegramBot.Controller;

namespace HotelTelegramBot.UnitTest
{
    [TestClass]
    public class ValidatorTests
    {
        [TestMethod]
        public void CheckDateFormat_CorectFormat_ReturnsTrue()
        {
            string userDate = "31.12.99";

            bool result = Validator.CheckDateFormat(userDate);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckDateFormat_IncorectFormat_ReturnsFalse()
        {
            string userDate = "3199";

            bool result = Validator.CheckDateFormat(userDate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckDateBigerCurrent_DateIsBefor_ReturnsFalse()
        {
            string userDate = "31.12.99";

            bool result = Validator.CheckDateBigerCurrent(userDate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckDateRange_EndDateIsLateStartDate_ReturnTrue()
        {
            string start = "11.02.2019";
            string end = "12.02.2019";

            bool result = Validator.CheckDateRange(start, end);

            Assert.IsTrue(result);
        }
    }
}
