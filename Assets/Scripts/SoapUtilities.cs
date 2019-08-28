using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VRTX.Net
{
    public class SoapUtilities
    {
        public static T Deserialize<T>(string xmlStr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(xmlStr))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
        public static XElement Serialize<T>(T value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, value);
            }
            return XElement.Parse(sb.ToString());
        }
    }

}