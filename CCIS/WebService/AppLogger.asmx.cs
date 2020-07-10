using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace CCIS.WebService
{
    /// <summary>
    /// Summary description for AppLogger
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AppLogger : System.Web.Services.WebService
    {

        [WebMethod]
        public string SaveLog(string _ApplicationName , string _AppPath , string _LogDetails ,string _ServerName,   bool _isCompressed      )
        {
            try
            {
                if (!_isCompressed)
                {
                    DAL.Operations.Logger.Log(_ApplicationName, _AppPath, "", _LogDetails,  _isCompressed.ToString(),_ServerName);
                }
                else
                {
                   // DAL.Operations.Logger.Info("Orginal string length :      " + _LogDetails.Length);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Decompress(_LogDetails));
                  //  Console.WriteLine("UnCompressed string length :      " + sb.ToString().Length);

                    DAL.Operations.Logger.Log(_ApplicationName, _AppPath, "", sb.ToString(), "", _isCompressed.ToString());

                }
                return "Success";

            }
            catch (Exception exc)
            {

                DAL.Operations.Logger.LogError(exc);
                return null;
            }
        }


        public static string Decompress(string input)
        {
            byte[] compressed = Convert.FromBase64String(input);
            byte[] decompressed = Decompress(compressed);
            return Encoding.UTF8.GetString(decompressed);
        }

        public static string Compress(string input)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(input);
            byte[] compressed = Compress(encoded);
            return Convert.ToBase64String(compressed);
        }

        public static byte[] Decompress(byte[] input)
        {
            using (var source = new MemoryStream(input))
            {
                byte[] lengthBytes = new byte[4];
                source.Read(lengthBytes, 0, 4);

                var length = BitConverter.ToInt32(lengthBytes, 0);
                using (var decompressionStream = new GZipStream(source,
                    CompressionMode.Decompress))
                {
                    var result = new byte[length];
                    decompressionStream.Read(result, 0, length);
                    return result;
                }
            }
        }

        public static byte[] Compress(byte[] input)
        {
            using (var result = new MemoryStream())
            {
                var lengthBytes = BitConverter.GetBytes(input.Length);
                result.Write(lengthBytes, 0, 4);

                using (var compressionStream = new GZipStream(result,
                    CompressionMode.Compress))
                {
                    compressionStream.Write(input, 0, input.Length);
                    compressionStream.Flush();

                }
                return result.ToArray();
            }
        }




    }
}
