using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entities;
using Newtonsoft.Json.Linq;

namespace DAL.Jira
{
    public class JiraStatuses
    {

        private JiraSynch jiraSynch = new JiraSynch();

        public static DataTable GetStatuses()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            try
            {
                string Json = Rest_API.API("status", "", "GET");
                //string result = JObject.Parse(Json).ToString();


                JArray yo = JArray.Parse(Json);
                int iteration = yo.Count;

                for (int j = 0; j < iteration; j++)
                {
                    dataTable.Rows.Add(yo[j]["id"].ToString(), yo[j]["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }

            return dataTable;
        }
        public string Synch_Status()
        {
            List<string> List_Tickets = new List<string>();
            bool changed = false;
            string result = string.Empty;

            try
            {
                List_Tickets = jiraSynch.FethcListofJiraTickets();

                foreach (string TicketKey in List_Tickets)
                {
                    //if (TicketKey != "TEST-55")
                    //{
                    //    continue;
                    //}


                    Entities.JiraTicket jiraTicket = DAL.Operations.OpJiraTicket.GetLatestByTicketKey(TicketKey);

                    if (jiraTicket != null)
                    {

                        if (jiraSynch.Jira_CheckTicketExist(TicketKey))
                        {
                            string InternalJiraStatus = jiraTicket.Status;
                            string ExternalJiraStatus = FetchJiraStatusByJiraKey(TicketKey);

                            if (InternalJiraStatus != null)
                            {
                                if (InternalJiraStatus.ToLower() != ExternalJiraStatus.ToLower())
                                {
                                    jiraTicket.Status = ExternalJiraStatus;
                                    changed = true;

                                    int ticketID = jiraSynch.GetTicketinformationIDByJiraKey(TicketKey);
                                    if (ExternalJiraStatus.ToUpper() == "CLOSED")
                                    {
                                        jiraSynch.CreateTicketHistory(ticketID, "JIRA: The status has been changed from [" + InternalJiraStatus + "] to [" + ExternalJiraStatus + "]", true, "JIRA STATUS CHANGE");

                                    }
                                    else
                                    {
                                        jiraSynch.CreateTicketHistory(ticketID, "JIRA: The status has been changed from [" + InternalJiraStatus + "] to [" + ExternalJiraStatus + "]", "JIRA STATUS CHANGE");
                                    }
                                    InsertTicket(jiraTicket);

                                    result += "<br/>TicketInformationID:" + ticketID + "  The status has been changed from " + InternalJiraStatus + " to " + ExternalJiraStatus;
                                }
                            }
                            
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        public void InsertTicket(Entities.JiraTicket jiraTicket)
        {
            try
            {
                jiraTicket.CreatedBy = "IQServiceDesk";
                //DAL.Operations.OpJiraTicket.InsertRecord(jiraTicket);
                ////jiraSynch.CreateNotification();

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        public string FetchJiraStatusByJiraKey(string key)
        {
            string result = string.Empty;

            try
            {
                string yo = DAL.Jira.Rest_API.API("issue/" + key, "", "GET", true);
                if (yo != "The remote server returned an error: (404) Not Found." && yo != "The remote server returned an error: (403) Forbidden.")
                {
                    JObject result2 = JObject.Parse(yo);
                    result = result2["fields"]["status"]["name"].ToString();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
    }
}
