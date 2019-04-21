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

        [TestMethod]
        public void CheckDateRange_StartDateIsLateEndDate_ReturnFalse()
        {
            string start = "12.02.2019";
            string end = "11.02.2019";

            bool result = Validator.CheckDateRange(start, end);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckNumber_ValidNumber_ReturnTrue()
        {
            string number = "11235";

            bool result = Validator.CheckNumber(number);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckNumber_InvalidNumber_ReturnFalse()
        {
            string number = "_11235";

            bool result = Validator.CheckNumber(number);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckNumberRange_NumberInRange_ReturnTrue()
        {
            string number = "9";

            bool result = Validator.CheckNumberRange(number);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckNumberRange_NumberOutOfRange_ReturnFalse()
        {
            string number = "999";

            bool result = Validator.CheckNumberRange(number);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckNumberRange_NegativeNumberOutOfRange_ReturnFalse()
        {
            string number = "-999";

            bool result = Validator.CheckNumberRange(number);

            Assert.IsFalse(result);
        }
    }
}
