using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DAL.Helper
{
  public static class XMLHelper
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return null;
            }
        }


        public static string getEntityXML(Entities.PersonInformation _MR)
        {
            return Serialize<Entities.PersonInformation>(_MR);
        }


        public  static  byte[] Base64XML(string _Xml)
        {

            return System.Text.Encoding.UTF8.GetBytes(_Xml);
        }

        public static string ByteToString(byte[] _byteArray)
        {

            

            return System.Text.Encoding.UTF8.GetString(_byteArray);
        }
    }
}
