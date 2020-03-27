using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VRTX.Net
{
    /// <summary>
    /// Helper class used to de-/serialize data types used in SOAP operations, requests and responses
    /// </summary>
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

    /// <summary>
    /// Helper class used to provide de-/serialization of data type directly from source types (string or any serializable type)
    /// </summary>
    public static class SoapExtensions
    {
        public static T Deserialize<T>(this string xmlStr)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(xmlStr))
            {
                result = (T)serializer.Deserialize(reader);
            }
            return result;
        }
        public static XElement Serialize<T>(this T value)
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