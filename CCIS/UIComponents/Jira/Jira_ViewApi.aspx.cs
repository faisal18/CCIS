using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CCIS.UIComponents.Jira
{
    public partial class Jira_ViewApi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (txt_resource.Text.Length > 0)
            {

                string result = string.Empty;
                string URL = txt_resource.Text;
                try
                {
                    switch (ddl_method.SelectedValue)
                    {
                        case "GET":
                            result = DAL.Jira.Rest_API.API(URL, "", "GET");
                            break;

                        case "POST":
                            result = DAL.Jira.Rest_API.API(URL, txt_richbox_postFile.InnerText, "POST");
                            break;

                        case "PUT":
                            result = DAL.Jira.Rest_API.API(URL, txt_richbox_postFile.InnerText, "PUT");
                            break;

                        case "DELETE":
                            result = DAL.Jira.Rest_API.API(URL, "", "DELETE");
                            break;

                    }

                    if (result.Trim().Substring(0, 1) == "[")
                    {
                        JArray array = JArray.Parse(result);
                        result = array.ToString();
                    }
                    if (result.Trim().Substring(0, 1) == "{")
                    {
                        result = JObject.Parse(result).ToString();
                    }

                    txt_richbox_output.InnerText = result.ToString();
                }
                catch (Exception ex)
                {
                    lbl_message.Text = ex.Message ;
                }
            }
        }

        //private string API(string URL,string username,string password,string data,string method)
        //{
        //    string result = string.Empty;
        //    try
        //    {
        //        using(WebClient wc = new WebClient())
        //        {
        //            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
        //            wc.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
        //            wc.Headers.Add("User-Agent: Other");

        //            switch (method.ToUpper().ToString())
        //            {
        //                case "GET":
        //                    result = wc.DownloadString(URL);
        //                    break;
        //                case "POST":
        //                    result = wc.UploadString(URL, data);
        //                    break;
        //                case "DELETE":
        //                    result = wc.UploadString(URL, method, data);
        //                    break;
        //                case "PUT":
        //                    result = wc.UploadString(URL, method, data);
        //                    break;
        //            }


        //            //Parse result as JSON
        //            if (result.Trim().Substring(0, 1) == "[")
        //            {
        //                JArray array = JArray.Parse(result);
        //                result = array.ToString();
        //            }
        //            if (result.Trim().Substring(0, 1) == "{")
        //            {
        //                result = JObject.Parse(result).ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }
        //    return result;
        //}
    }
}