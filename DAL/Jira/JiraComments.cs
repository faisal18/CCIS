using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entities;
using Newtonsoft.Json.Linq;

namespace DAL.Jira
{
    class JiraComments
    {
        private JiraSynch jiraSynch = new JiraSynch();

        public string SynchComments()
        {
            DAL.Operations.Logger.Info("Jira SynchComments has been called");
            List<string> List_Tickets = new List<string>();
            List<string> List_Comments = new List<string>();
            List<string> List_JiraComments = new List<string>();
            List<string> Jira_Keysnotexist = new List<string>();

            string result2 = string.Empty;

            try
            {
                
                List_Tickets = jiraSynch.FethcListofJiraTickets();


                foreach (string JiraTicketsKey in List_Tickets)
                {

                    //if (JiraTicketsKey != "EAUTH-198")
                    //{
                    //    continue;
                    //}

                    DAL.Operations.Logger.Info("Synching ticket for JiraTicket " + JiraTicketsKey);

                    Entities.JiraTicketComments jiraTicketComments = DAL.Operations.OpJiraTicketComments.GetLatestByTicketKey(JiraTicketsKey);

                    if (jiraTicketComments != null)
                    {
                        if (jiraSynch.Jira_CheckTicketExist(JiraTicketsKey))
                        {
                            int TicketInformationID = jiraSynch.GetTicketinformationIDByJiraKey(JiraTicketsKey);
                            List_Comments = FetchSystemCommentsByJiraKey(JiraTicketsKey);
                            List_JiraComments = FetchJiraCommentsByJiraKey(JiraTicketsKey);


                            var result = List_JiraComments.Except(List_Comments, StringComparer.OrdinalIgnoreCase).ToList();

                            if (result.Count > 0)
                            {
                                //break;
                                DAL.Operations.Logger.Info("Found " + result.Count + " new comments for this jiraticket");
                                for (int i = 0; i < result.Count; i++)
                                {

                                    string author = FetchJiraCommentAuthorByJiraKey(JiraTicketsKey, result[i]);

                                    jiraSynch.CreateTicketHistory(TicketInformationID, result[i], "JIRA COMMENTS UPDATED", author);
                                    jiraTicketComments.Comments = result[i];
                                    InsertComments(jiraTicketComments);

                                    result2 += "<br/>Added comment to Ticket " + TicketInformationID + " having JiraTicketID " + JiraTicketsKey;
                                }
                            }
                        }
                        else
                        {
                            Jira_Keysnotexist.Add(JiraTicketsKey);
                        }
                    }


                }

                Console.WriteLine(Jira_Keysnotexist.ToList());

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }

            return result2;
        }

        public string SynchCommentsbyTicket(string JiraTicketsKey)

        {
            List<string> List_Tickets = new List<string>();
            List<string> List_Comments = new List<string>();
            List<string> List_JiraComments = new List<string>();
            List<string> Jira_Keysnotexist = new List<string>();

            string result2 = string.Empty;

            try
            {

                //List_Tickets = jiraSynch.FethcListofJiraTickets();



                Entities.JiraTicketComments jiraTicketComments = DAL.Operations.OpJiraTicketComments.GetLatestByTicketKey(JiraTicketsKey);

                if (jiraTicketComments != null)
                {
                    if (jiraSynch.Jira_CheckTicketExist(JiraTicketsKey))
                    {
                        int TicketInformationID = jiraSynch.GetTicketinformationIDByJiraKey(JiraTicketsKey);
                        List_Comments = FetchSystemCommentsByJiraKey(JiraTicketsKey);
                        List_JiraComments = FetchJiraCommentsByJiraKey(JiraTicketsKey);


                        var result = List_JiraComments.Except(List_Comments, StringComparer.OrdinalIgnoreCase).ToList();

                        if (result.Count > 0)
                        {
                            //break;
                            for (int i = 0; i < result.Count; i++)
                            {

                                string author = FetchJiraCommentAuthorByJiraKey(JiraTicketsKey, result[i]);

                                jiraSynch.CreateTicketHistory(TicketInformationID, result[i], "JIRA COMMENTS UPDATED", author);
                                jiraTicketComments.Comments = result[i];
                                InsertComments(jiraTicketComments);

                                result2 += "<br/>Added comment to Ticket " + TicketInformationID + " having JiraTicketID " + JiraTicketsKey;
                            }
                        }
                    }
                    else
                    {
                        Jira_Keysnotexist.Add(JiraTicketsKey);
                    }
                }


                

                Console.WriteLine(Jira_Keysnotexist.ToList());

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }

            return result2;
        }
        

        public void InsertComments(Entities.JiraTicketComments jiraComments)
        {
            try
            {
                DAL.Operations.Logger.Info("Adding comment \"" + jiraComments.Comments + "\" to ticketID " + jiraComments.TicketInformationID);
                jiraComments.CreatedBy = "IQServiceDesk";
                DAL.Operations.OpJiraTicketComments.InsertRecord(jiraComments);
                jiraSynch.CreateNotification(jiraComments.TicketInformationID, jiraComments.Comments);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private string FetchJiraCommentAuthorByJiraKey(string Key, string findcomment)
        {
            string result = string.Empty;
            string author = string.Empty;
            List<string> list_comments = new List<string>();

            try
            {


                result = DAL.Jira.Rest_API.API("issue/" + Key + "/comment", "", "GET");
                if (result != "The remote server returned an error: (404) Not Found." && result != "The remote server returned an error: (403) Forbidden.")
                {
                    JObject result2 = JObject.Parse(result);
                    int count = int.Parse(result2["total"].ToString());
                    for (int i = 0; i < count; i++)
                    {
                        if (findcomment == result2["comments"][i]["body"].ToString())
                        {
                            author = result2["comments"][i]["author"]["displayName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return author;
        }
        private List<string> FetchJiraCommentsByJiraKey(string Key)
        {
            string result = string.Empty;
            List<string> list_comments = new List<string>();

            try
            {


                result = DAL.Jira.Rest_API.API("issue/" + Key + "/comment", "", "GET");
                if (result != "The remote server returned an error: (404) Not Found." && result != "The remote server returned an error: (403) Forbidden.")
                {
                    JObject result2 = JObject.Parse(result);

                    int count = int.Parse(result2["total"].ToString());
                    for (int i = 0; i < count; i++)
                    {
                        //list_comments.Add(jiraSynch.Jira_EscapeCharacters(result2["comments"][i]["body"].ToString(), true));

                        list_comments.Add(result2["comments"][i]["body"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return list_comments;
        }
        private List<string> FetchSystemCommentsByJiraKey(string Key)
        {
            List<string> List_Comments = new List<string>();
            try
            {
                List_Comments = DAL.Operations.OpJiraTicketComments.GetAll().Where(x => x.JiraTicketKey == Key).OrderBy(x => x.Comments).Select(x => x.Comments).Distinct().ToList();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return List_Comments;
        }

    }


}
