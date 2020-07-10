using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Jira
{
    public class JiraProjects
    {
        public static DataTable GetAllApplication()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));

            try
            {
                string Json = Rest_API.API("project", "", "GET");

                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["id"].ToString(), yo[j]["key"].ToString(), yo[j]["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }

        public static DataTable GetApplicationByIdOrKey(string Id)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Key", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));

            try
            {
                string Json = Rest_API.API("project/" + Id, "", "GET");

                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["id"].ToString(), yo[j]["key"].ToString(), yo[j]["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;

        }

        public static bool DeleteProject(string Id)
        {
            bool deleted = false;
            try
            {
                string Json = Rest_API.API("project/" + Id, "", "DELETE");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return deleted;
        }
    }
}
