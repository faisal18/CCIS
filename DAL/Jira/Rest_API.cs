
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace DAL.Jira
{
    public class Rest_API
    {

        public static string API(string resource, string data, string method)
        {
            string result = string.Empty;

            string URL = System.Configuration.ConfigurationManager.AppSettings.Get("JiraURL");
            string URLResource = System.Configuration.ConfigurationManager.AppSettings.Get("JiraResource");
            string username = System.Configuration.ConfigurationManager.AppSettings.Get("JiraUsername");
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("JiraPassword");
            URL = URL + URLResource + resource;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    wc.Headers.Add("User-Agent: Other");

                    switch (method.ToUpper())
                    {
                        case "GET":
                            result = wc.DownloadString(URL);

                            break;
                        case "POST":
                            result = wc.UploadString(URL, data);

                            break;
                        case "DELETE":
                            result = wc.UploadString(URL, method, data);

                            break;
                        case "PUT":
                            result = wc.UploadString(URL, method, data);

                            break;
                    }

                }
            }
            catch (WebException ex)
            {
                //result = ex.Message.ToString();
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            catch (Exception ex)
            {
                //lbl_message.Text = ex.Message;
                result = ex.Message;
            }
            return result;
        }

        public static string API(string resource, string filename)
        {
            string result = string.Empty;

            string URL = System.Configuration.ConfigurationManager.AppSettings.Get("JiraURL");
            string URLResource = System.Configuration.ConfigurationManager.AppSettings.Get("JiraResource");
            string username = System.Configuration.ConfigurationManager.AppSettings.Get("JiraUsername");
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("JiraPassword");
            URL = URL + URLResource + resource;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    wc.Headers.Add("User-Agent: Other");
                    wc.Headers.Add("X-Atlassian-Token:no-check");
                    wc.Headers.Add("Content-Type:multipart/form-data");


                    byte[] B_result = wc.UploadFile(URL, filename);
                    result = Encoding.UTF8.GetString(B_result);
                }
            }
            catch (WebException ex)
            {
                result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static string API(string resource, string data, string method, bool shortmessage)
        {
            string result = string.Empty;

            string URL = System.Configuration.ConfigurationManager.AppSettings.Get("JiraURL");
            string URLResource = System.Configuration.ConfigurationManager.AppSettings.Get("JiraResource");
            string username = System.Configuration.ConfigurationManager.AppSettings.Get("JiraUsername");
            string password = System.Configuration.ConfigurationManager.AppSettings.Get("JiraPassword");
            URL = URL + URLResource + resource;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                    wc.Headers.Add("User-Agent: Other");

                    switch (method.ToUpper())
                    {
                        case "GET":
                            result = wc.DownloadString(URL);

                            break;
                        case "POST":
                            result = wc.UploadString(URL, data);

                            break;
                        case "DELETE":
                            result = wc.UploadString(URL, method, data);

                            break;
                        case "PUT":
                            result = wc.UploadString(URL, method, data);

                            break;
                    }

                }
            }
            catch (WebException ex)
            {
                result = ex.Message.ToString();
                //result = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
            }

            catch (Exception ex)
            {
                //lbl_message.Text = ex.Message;
                result = ex.Message;
            }
            return result;
        }

    }
}
