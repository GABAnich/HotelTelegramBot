using System.Xml;

namespace HotelTelegramBot
{
    class ConfigTelegramBot
    {
        public static string APIToken
        {
            get
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(@"..\..\Config.xml");

                return xDoc.SelectSingleNode("//config/APIToken").InnerText;
            }
        }

        public static string LogFile
        {
            get
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(@"..\..\Config.xml");

                return xDoc.SelectSingleNode("//config/LogFile").InnerText;
            }
        }
    }
}
