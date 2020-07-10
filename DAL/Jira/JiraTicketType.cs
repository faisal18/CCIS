using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Jira
{
    public class JiraTicketType
    {
        public static DataTable GetAllTicketType(string ApplicationID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            try
            {
                //string Json = Rest_API.API("issuetype", "", "GET");

                string Json = Rest_API.API("project/" + ApplicationID, "", "GET");


                JObject jObject = JObject.Parse(Json);

                JArray yo = JArray.Parse(jObject["issueTypes"].ToString());
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["id"].ToString(), yo[j]["name"].ToString(), yo[j]["description"].ToString());
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }

        public static DataTable GetTicketTypeById(string ID)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            try
            {
                string Json = Rest_API.API("issuetype/" + ID, "", "GET");

                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["id"].ToString(), yo[j]["name"].ToString(), yo[j]["description"].ToString());
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return dataTable;

        }


    }
}
