using System.Xml;

namespace HotelTelegramBot.Model
{
    class AboutHotel
    {
        public static string InfoAboutHotel
        {
            get
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(@"..\..\Model\AboutHotel.xml");

                return xDoc.SelectSingleNode("//data/infoAboutHotel").InnerText;
            }
        }

        public static string ImageAboutHotel
        {
            get
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(@"..\..\Model\AboutHotel.xml");

                return xDoc.SelectSingleNode("//data/imageAboutHotel").InnerText;
            }
        }
    }
}
