using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

namespace CCIS.UIComponents.Admin
{
    public partial class GridTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static string basic()
        {
            string result = JObject.Parse("{     \"current\": 1,     \"rowCount\": 10,     \"rows\": [       {         \"id\": 19,         \"sender\": \"123@test.de\",         \"received\": \"2014-05-30T22:15:00\"       },       {         \"id\": 14,         \"sender\": \"123@test.de\",         \"received\": \"2014-05-30T20:15:00\"       },       ...     ],     \"total\": 1123   }").ToString();


            return result;
        }
    }
}