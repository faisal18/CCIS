using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Jira
{
    public class JiraUsers
    {
        public static DataTable GetAssignableUserByApplicationKey(string AppKey)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            try
            {
                string Json = Rest_API.API("user/assignable/search?project=" + AppKey, "", "GET");

                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["key"].ToString(), yo[j]["displayName"].ToString(), yo[j]["emailAddress"].ToString());
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return dataTable;
        }

        public static DataTable GetAssignableUserByTicketKey(string TicketKey)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            try
            {
                string Json = Rest_API.API("user/assignable/search?issueKey=" + TicketKey, "", "GET");

                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["key"].ToString(), yo[j]["displayName"].ToString(), yo[j]["emailAddress"].ToString());
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
