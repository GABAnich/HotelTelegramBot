using System.Xml;

namespace HotelTelegramBot
{
    class ConfigTelegramBot
    {
        internal static string APIToken
        {
            get
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(@"..\..\Config.xml");

                return xDoc.SelectSingleNode("//config/APIToken").InnerText;
            }
        }

        internal static string LogFile
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
