using System.Xml;

namespace HotelTelegramBot
{
    class ConfigTelegramBot
    {
        static public string GetAPIToken()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"..\..\Config.xml");

            return xDoc.SelectSingleNode("//config/APIToken").InnerText;
        }
    }
}
