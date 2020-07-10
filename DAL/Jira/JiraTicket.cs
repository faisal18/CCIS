using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Jira
{
    public class JiraTicket
    {
        //Issue
        //Comment
        //Assign
        //Attachment


        public static string GetIssuebyKey(string key)
        {
            string result = string.Empty;
            try
            {
                string Json = Rest_API.API("issue/" + key, "", "GET");
                result = JObject.Parse(Json).ToString();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return ex.Message;
            }

            return result;
        }

        public void InsertRecord()
        {
            try
            {

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        public void CheckStatus()
        {
            List<string> List_Tickets = new List<string>();
            try
            {
                List_Tickets.Add("");
                List_Tickets.Add("");
                List_Tickets.Add("");

                foreach (string TicketKey in List_Tickets)
                {
                    //pick latest ticket status from JIRATicketTable

                    //get status from JIRA API 

                    //compare statuses

                    //if not same insert record in JIRATicketTable and add TicketHistory with comments of ticket status
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        public void CheckComments()
        {
            List<string> List_Tickets = new List<string>();
            List<string> List_Comments = new List<string>();
            List<string> List_JiraComments = new List<string>();
            try
            {
                //get list of tickets
                List_Tickets.Add("");
                List_Tickets.Add("");

                foreach (string ticket in List_Tickets)
                {
                    //add all comments for each tickets
                    List_Comments.Add("");

                    //get list of comments for this ticket from JIRA
                    List_JiraComments.Add("");

                    var result = List_JiraComments.Except(List_Comments, StringComparer.OrdinalIgnoreCase).ToList();

                    //add result to JiraCommentsTable
                }

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}
